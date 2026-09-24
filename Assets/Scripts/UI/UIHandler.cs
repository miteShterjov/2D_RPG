using UnityEngine;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public UISkillToolTip skillToolTip;
        
        private void Awake()
        {
            skillToolTip = GetComponentInChildren<UISkillToolTip>();
        }
    }
}