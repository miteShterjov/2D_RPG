using SkillSystem;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UISkillToolTip : UIToolTip
    {
        [ Header("Skill ToolTip")]
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI skillDescription;
        [SerializeField] private TextMeshProUGUI skillRequirements;

        public override void ShowToolTip(bool showToolTip, RectTransform targetRectTransform)
        {
            base.ShowToolTip(showToolTip, targetRectTransform);
        }

        public void ShowToolTip(bool show, RectTransform targetRectTransform, SkillDataSO skillData)
        {
            base.ShowToolTip(show, targetRectTransform);
            if (!show) return;

            skillName.text = skillData.skillName;
            skillDescription.text = skillData.description;
            skillRequirements.text = "Requirements: \n "
                + " -" + skillData.skillCost + " skill points.";
        }
    }
}