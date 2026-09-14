using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyVFX : EntityVFX
    {
        [Header("Counter-Attack VFX")] 
        [SerializeField] private GameObject attackAlert;
        
        // Enables a visual sign above the enemy health bar that a counter-attack 
        // window is open for the player to execute the attack. The current alert is a 
        // red exclamation mark. 
        public void EnableAttackAlert(bool enable)
        {
            if (attackAlert != null) attackAlert.SetActive(enable);
        }
    }
}
