using System.Collections.Generic;
using System.Linq;
using SkillSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [Header("Skill details")]
        [SerializeField] private string skillName;
        [SerializeField] private int skillCost;
        [SerializeField] private Image skillIcon;
        [SerializeField] private Color lockedColor;
        [SerializeField] public SkillDataSO skillData;

        [Header("Unlock Details")] 
        public UITreeNode[] neededNodes;
        public UITreeNode[] conflictedNodes;
        public bool isUnlocked;
        public bool isLocked;
        
        private UIHandler uiHandler;
        private RectTransform rectTransform;
        private UISkillTree skillTree;
        private UITreeConnectionHandler connectionHandler;

        private Color lastColor;
        private Material originalIconMaterial;
        private Material grayscaleIconMaterial;
        private const string LockedColorHexString = "#9F9797";
        private static readonly int GrayscalePropertyId = Shader.PropertyToID("_Grayscale");

        private void Awake()
        {
            uiHandler = GetComponentInParent<UIHandler>();
            rectTransform = GetComponent<RectTransform>();
            skillTree = GetComponentInParent<UISkillTree>();
            connectionHandler = GetComponent<UITreeConnectionHandler>();

            if (skillIcon)
            {
                Shader grayscaleShader = Shader.Find("UI/SkillIconGrayscale");
                if (!grayscaleShader)
                {
                    Debug.LogError("UI/SkillIconGrayscale shader could not be found.", this);
                }
                else
                {
                    originalIconMaterial = skillIcon.material;
                    grayscaleIconMaterial = new Material(grayscaleShader);
                    skillIcon.material = grayscaleIconMaterial;
                }
            }

            UpdateSkillIconColor(isUnlocked ? Color.white : GetColorFromHex(LockedColorHexString));
            UpdateSkillIconGrayscale(!isUnlocked);
        }
        
        private void Start()
        {
            if (skillData.isUnlockedByDefault) UnlockSkillPoint();
        }
        
        private void OnValidate()
        {
            if (!skillData) return;
            skillName = skillData.skillName;
            skillCost = skillData.skillCost;
            skillIcon.sprite = skillData.icon;
            gameObject.name = "UITreeNode - " + skillData.skillName;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {   
            uiHandler.skillToolTip.ShowToolTip(true, rectTransform, this);
            
            if (!isUnlocked || !isLocked) return;
            ToggleNodeHighlight(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiHandler.skillToolTip.ShowToolTip(false, rectTransform);
            
            if (!isUnlocked || !isLocked) return;
            ToggleNodeHighlight(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CanBeUnlocked()) UnlockSkillPoint();
            else if(isLocked) uiHandler.skillToolTip.LockedSkillEffect();
        }

        public void RefundsSkillPoints()
        {
            isUnlocked = false;
            isLocked = false;
            UpdateSkillIconColor(GetColorFromHex(LockedColorHexString));
            UpdateSkillIconGrayscale(true);
            skillTree.AddSkillPoints(skillData.skillCost);
            connectionHandler.ConnectionImageUnlocked(false);
        }

        public void LockChildNodes()
        {
             isLocked = true;

             foreach (UITreeNode node in connectionHandler.GetAllChildNodes())
             {
                 node.LockChildNodes();
             }
        }

        private void OnDisable()
        {
            if(isLocked) UpdateSkillIconColor(GetColorFromHex(LockedColorHexString));
            if(isUnlocked) UpdateSkillIconColor(Color.white);
            UpdateSkillIconGrayscale(!isUnlocked);
        }

        private void OnDestroy()
        {
            if (skillIcon && skillIcon.material == grayscaleIconMaterial)
                skillIcon.material = originalIconMaterial;

            if (!grayscaleIconMaterial) return;
            if (Application.isPlaying) Destroy(grayscaleIconMaterial);
            else DestroyImmediate(grayscaleIconMaterial);
        }

        private bool CanBeUnlocked()
        {
            if (isLocked || isUnlocked) return false;
            
            if (!skillTree.HasEnoughSkillPoints(skillData.skillCost)) return false;

            if (neededNodes != null && neededNodes.Any(node => !node || !node.isUnlocked))
            {
                return false;
            }

            return conflictedNodes == null || conflictedNodes.All(node => !node || !node.isUnlocked);
        }

        private void UnlockSkillPoint()
        {
            isUnlocked = true;
            UpdateSkillIconColor(Color.white);
            UpdateSkillIconGrayscale(false);
            LockConflictedNodes();
            skillTree.SpendSkillPoints(skillData.skillCost);
            connectionHandler.ConnectionImageUnlocked(true);

            if (skillData.upgradeData == null)
            {
                Debug.LogWarning($"Talent '{skillData.skillName}' has no upgrade data assigned.", this);
            }
            else
            {
                skillTree.SkillsManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
            }
        }

        private void UpdateSkillIconColor(Color color)
        {
            if (!skillIcon) return;
            lastColor = skillIcon.color;
            skillIcon.color = color;
        }

        private void UpdateSkillIconGrayscale(bool isGrayscale)
        {
            if (grayscaleIconMaterial)
                grayscaleIconMaterial.SetFloat(GrayscalePropertyId, isGrayscale ? 1f : 0f);
        }

        private void ToggleNodeHighlight(bool isHighlighted)
        {
            Color highlightColor = Color.white * 0.9f;
            highlightColor.a = 1f;
            Color colorToApply = isHighlighted ? highlightColor : lastColor;
            
            UpdateSkillIconColor(colorToApply);
        }
        
        private Color GetColorFromHex(string hex) => ColorUtility.TryParseHtmlString(hex, out Color color) ? color : Color.white;
        
        private void LockConflictedNodes()
        {
            if (conflictedNodes == null) return;

            foreach (UITreeNode conflictedNode in conflictedNodes)
            {
                if (!conflictedNode) continue;
                conflictedNode.isLocked = true;
                conflictedNode.LockChildNodes();
                conflictedNode.UpdateSkillIconColor(lockedColor);
            }
        }

        public IEnumerator<UITreeNode> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}