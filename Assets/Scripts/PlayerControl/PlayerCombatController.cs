using EntityControl;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerCombatController : EntityCombatController
    {
        [Space]
        [SerializeField] private float counterRecovery = 0.25f;
        
        public bool CounterAttackPerformed()
        {
            bool hasPreformedCounter = false;
            GetDetectedColliders();
            
            foreach (Collider2D target in targetColliders)
            {
                ICounterable counterable = target.GetComponent<ICounterable>();
                if (counterable == null) continue;
                
                if (counterable.CanBeCountered)
                {
                    counterable.HandleCounterAttack();
                    hasPreformedCounter = true;
                }
            }

            return hasPreformedCounter;
        }

        public float GetCounterRecoveryDuration() => counterRecovery;
    }
}