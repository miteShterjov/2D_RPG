using UnityEngine;

namespace PlayerControl
{
    public class PlayerAnimTriggersHandler : MonoBehaviour
    {
        private PlayerController _player;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
        }

        public void CurrentStateAnimTrigger() => _player.CallAnimTrigger();
    }
}