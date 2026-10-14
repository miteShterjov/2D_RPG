using System;
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
        private const string LockedColorHexString = "#9F9797";

        private void Awake()
        {
            uiHandler = GetComponentInParent<UIHandler>();
            rectTransform = GetComponent<RectTransform>();
            skillTree = GetComponentInParent<UISkillTree>();
            connectionHandler = GetComponent<UITreeConnectionHandler>();
            
            UpdateSkillIconColor(GetColorFromHex(LockedColorHexString));
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
            skillTree.AddSkillPoints(skillData.skillCost);
            connectionHandler.ConnectionImageUnlocked(false);
        }

        private void OnDisable()
        {
            if(isLocked) UpdateSkillIconColor(GetColorFromHex(LockedColorHexString));
            if(isUnlocked) UpdateSkillIconColor(Color.white);
        }

        private bool CanBeUnlocked()
        {
            if (isLocked || isUnlocked) return false;
            
            if (!skillTree.HasEnoughSkillPoints(skillData.skillCost)) return false;

            if (neededNodes.Any(node => !node.isUnlocked))
            {
                return false;
            }

            return conflictedNodes.All(node => !node.isUnlocked);
        }

        private void UnlockSkillPoint()
        {
            isUnlocked = true;
            UpdateSkillIconColor(Color.white);
            LockConflictedNodes();
            skillTree.SpendSkillPoints(skillData.skillCost);
            connectionHandler.ConnectionImageUnlocked(true);
        }

        private void UpdateSkillIconColor(Color color)
        {
            if (!skillIcon) return;
            lastColor = skillIcon.color;
            skillIcon.color = color;
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
            foreach (UITreeNode conflictedNode in conflictedNodes)
            {
                conflictedNode.isLocked = true;
                UpdateSkillIconColor(lockedColor);
            }
        }

        public IEnumerator<UITreeNode> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}