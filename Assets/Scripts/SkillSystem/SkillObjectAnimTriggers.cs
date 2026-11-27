using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectAnimTriggers : MonoBehaviour
    {
        private SkillObjectTimeEcho timeEcho;

        private void Awake()
        {
            timeEcho = GetComponentInParent<SkillObjectTimeEcho>();
            if (timeEcho == null)
                Debug.LogError("SkillObjectTimeEcho not found in parent hierarchy.", this);
        }

        public void AttackTriggerAnimEvent()
        {
            if (timeEcho != null) timeEcho.PreformAttack();
        }

        public void TryTerminate(int currentAttackIndex)
        {
            if (timeEcho != null) timeEcho.TryTerminate(currentAttackIndex);
        }
    }
}