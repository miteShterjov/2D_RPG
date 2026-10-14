using UnityEngine;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public UISkillToolTip skillToolTip;
        public UISkillTree skillTree;

        private void Awake()
        {
            skillToolTip = GetComponentInChildren<UISkillToolTip>();
            skillTree = GetComponentInChildren<UISkillTree>(true);
        }

        public void ToggleSkillTree()
        {
            if (!skillTree)
            {
                Debug.LogError("UIHandler could not find a UISkillTree to toggle.", this);
                return;
            }

            skillTree.gameObject.SetActive(!skillTree.gameObject.activeSelf);

            if (skillToolTip) skillToolTip.ShowToolTip(false, null);
        }
    }
}