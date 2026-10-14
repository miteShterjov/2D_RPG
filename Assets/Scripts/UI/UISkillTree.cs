using System;
using UnityEngine;

namespace UI
{
    public class UISkillTree : MonoBehaviour
    {
        [ Header("Skill Tree")]
        [SerializeField] private int skillPoints;
        [SerializeField] private UITreeConnectionHandler[] parentNodes;

        private void Start()
        {
            UpdateAllConnections();
        }

        public bool HasEnoughSkillPoints(int cost) => skillPoints >= cost;
        public void SpendSkillPoints(int cost) => skillPoints -= cost;
        
        public void AddSkillPoints(int points) => skillPoints += points;

        [ContextMenu("Reset Skills Tree")]
        public void RefundAllSkillPoints()
        {
            UITreeNode skillNodes = GetComponentInChildren<UITreeNode>();
            foreach (UITreeNode node in skillNodes)
            {
                node.RefundsSkillPoints();
            }
        }
        
        [ContextMenu("Update All Connections")]
        public void UpdateAllConnections()
        {
            foreach (UITreeConnectionHandler parentNode in parentNodes)
            {
                if (!parentNode) continue;
                parentNode.UpdateAllConnections();
            }
        }
    }
}