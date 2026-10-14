using System.Collections;
using System.Text;
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
        [Space]
        [SerializeField] private string metConditionsHexColor = "#00FF00";
        [SerializeField] private string unmetConditionsHexColor = "#FF0000";
        [SerializeField] private string importantInfoColor = "#FFFF00";
        [SerializeField] private Color colorPalette;
        [SerializeField] private string lockedOutSkillFlavour =
            "You have chosen a different path. This skill is perma locked out.";
        
        private UISkillTree skillTree;
        private UIHandler uiHandler;

        private Coroutine textEffectCo;

        protected override void Awake()
        {
            base.Awake();
            uiHandler = GetComponentInParent<UIHandler>();
            skillTree = uiHandler.GetComponentInChildren<UISkillTree>(true);
        }
        
        public override void ShowToolTip(bool showToolTip, RectTransform targetRectTransform)
        {
            base.ShowToolTip(showToolTip, targetRectTransform);
        }

        public void ShowToolTip(
            bool show, 
            RectTransform targetRectTransform, 
            UITreeNode treeNode)
        {
            base.ShowToolTip(show, targetRectTransform);
            if (!show) return;

            skillName.text = treeNode.skillData.skillName;
            skillDescription.text = treeNode.skillData.description;
            
            string skillLockedOut = $"<color={importantInfoColor}><b>{lockedOutSkillFlavour}</b></color>";
            string requirements = treeNode.isLocked ? skillLockedOut : GetRequirements(treeNode.skillData.skillCost, treeNode.neededNodes, treeNode.conflictedNodes);

            skillRequirements.text = requirements;
        }

        public void LockedSkillEffect()
        {
            if (textEffectCo != null) StopCoroutine(textEffectCo);
            textEffectCo = StartCoroutine(
                ShowToolTipTextEffect(skillRequirements, 0.2f, 3));
        }

        private string GetRequirements(int skillCost, UITreeNode[] neededNodes, UITreeNode[] conflictingNodes = null)
        {
            StringBuilder sb = new StringBuilder();
            string costColor = skillTree.HasEnoughSkillPoints(skillCost) ? metConditionsHexColor : unmetConditionsHexColor;
            string costText = $"- {skillCost} skill point(s).";
            string finalCostText = GetColoredText(costColor, costText);
            
            sb.AppendLine("Requirements:");
            sb.AppendLine(finalCostText);
            
            foreach (UITreeNode node in neededNodes)
            {
                string nodeColor = node.isUnlocked ? metConditionsHexColor : unmetConditionsHexColor;
                sb.AppendLine($"<color={nodeColor}> -{node.skillData.skillName}.</color>");
            }

            if (conflictingNodes != null)
            {
                sb.AppendLine();
                sb.AppendLine($"<color={importantInfoColor}>Locks out following skill(s):</color>");
                foreach (UITreeNode node in conflictingNodes)
                {
                    sb.AppendLine($"<color={importantInfoColor}> - {node.skillData.skillName}.</color>");
                }
            }
            
            return sb.ToString();
        }
        
        private IEnumerator ShowToolTipTextEffect(TextMeshProUGUI text, float blinkInterval, int blinkCount)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                text.text = GetColoredText(unmetConditionsHexColor, lockedOutSkillFlavour);
                yield return new WaitForSeconds(blinkInterval);
                text.text = GetColoredText(importantInfoColor, lockedOutSkillFlavour);
                yield return new WaitForSeconds(blinkInterval);
            }
        }
    }
}