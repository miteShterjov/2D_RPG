using EntityControl;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerAnimTriggersHandler : EntityAnimTriggersHandler
    {
        private PlayerController playerController;

        protected override void Awake()
        {
            base.Awake();
            
            playerController = GetComponentInParent<PlayerController>();
            if (playerController == null) Debug.LogError("PlayerController not found");
        }
        
        private void ThrowSword() => playerController.skillsManager.SwordThrow.ThrowSword();
    }
}