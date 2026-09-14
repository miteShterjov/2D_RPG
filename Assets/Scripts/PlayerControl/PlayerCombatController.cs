using System.Collections.Generic;
using EntityControl;
using Interface;
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
            HashSet<ICounterable> counteredTargets = new HashSet<ICounterable>();
            
            foreach (Collider2D target in targetColliders)
            {
                ICounterable counterable = target.GetComponentInParent<ICounterable>();
                if (counterable == null || !counteredTargets.Add(counterable)) continue;
                
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