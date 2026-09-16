using System;
using System.Collections;
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

        private PlayerMoveController moveController;
        private PlayerCollisionController collisionController;
        private PlayerController playerController;

        protected override void Awake()
        {
            base.Awake();
            moveController = GetComponent<PlayerMoveController>();
            collisionController = GetComponent<PlayerCollisionController>();
            playerController = GetComponent<PlayerController>();
        }

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

        protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
        {
            float originalMoveSpeed = moveController.moveSpeed;
            float originalJumpForce = moveController.jumpForce;
            Vector2 originalWallJump = moveController.wallJumpAngle;
            Vector2 originalJumpAttack = playerController.jumpAttackVelocity;
            Vector2[] originalAttackVelocity = new Vector2[playerController.attackVelocity.Length];
            Array.Copy(
                playerController.attackVelocity, 
                originalAttackVelocity, 
                playerController.attackVelocity.Length);
            
            float speedMultiplier = 1 - slowMultiplier;
            
            moveController.moveSpeed *= speedMultiplier;
            moveController.jumpForce *= speedMultiplier;
            moveController.wallJumpAngle *= speedMultiplier;
            playerController.jumpAttackVelocity *= speedMultiplier;
            playerController.jumpAttackVelocity *= speedMultiplier;

            for (int i = 0; i < playerController.attackVelocity.Length; i++)
            {
                playerController.attackVelocity[i] *= speedMultiplier;
            }

            yield return new WaitForSeconds(duration);

            moveController.moveSpeed = originalMoveSpeed;
            moveController.jumpForce = originalJumpForce;
            moveController.wallJumpAngle = originalWallJump;
            playerController.jumpAttackVelocity = originalJumpAttack;
            
            for (int i = 0; i < playerController.attackVelocity.Length; i++)
            {
                playerController.attackVelocity[i] *= originalAttackVelocity[i];
            }
        }
    }
}