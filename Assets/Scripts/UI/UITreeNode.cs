using SkillSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [Header("Skill Points")]
        [SerializeField] private string skillName;
        [SerializeField] private Image skillIcon;
        [SerializeField] private Color lockedColor;
        [SerializeField] private SkillDataSO skillData;
        
        private UIHandler uiHandler;
        private RectTransform rectTransform;

        private Color lastColor;
        private const string LockedColorHexString = "#9F9797";

        public bool isUnlocked;
        public bool isLocked;

        private void Awake()
        {
            uiHandler = GetComponentInParent<UIHandler>();
            rectTransform = GetComponent<RectTransform>();
            
            UpdateSkillIconColor(GetColorFromHex(LockedColorHexString));
        }
        
        private void OnValidate()
        {
            if (!skillData) return;
            skillName = skillData.skillName;
            skillIcon.sprite = skillData.icon;
            gameObject.name = "UITreeNode - " + skillData.skillName;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {   
            uiHandler.skillToolTip.ShowToolTip(true, rectTransform, skillData);
            
            if (isUnlocked) return;
            UpdateSkillIconColor(Color.white * 0.9f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiHandler.skillToolTip.ShowToolTip(false, rectTransform);
            
            if (isUnlocked) return;
            UpdateSkillIconColor(lastColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CanBeUnlocked()) UnlockSkillPoint();
            else Debug.Log("Skill point cannot be unlocked!");
        }

        private bool CanBeUnlocked()
        {
            if (isLocked || isUnlocked) return false;
            else return true;
        }

        private void UnlockSkillPoint()
        {
            isUnlocked = true;
            UpdateSkillIconColor(Color.white);
        }

        private void UpdateSkillIconColor(Color color)
        {
            if (!skillIcon) return;
            lastColor = skillIcon.color;
            skillIcon.color = color;
        }
        
        private Color GetColorFromHex(string hex) => ColorUtility.TryParseHtmlString(hex, out Color color) ? color : Color.white;
    }
}