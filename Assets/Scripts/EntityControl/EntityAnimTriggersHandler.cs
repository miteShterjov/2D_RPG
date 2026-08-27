using UnityEngine;

namespace EntityControl
{
    public class EntityAnimTriggersHandler : MonoBehaviour
    {
        private EntityController _entity;

        private void Awake()
        {
            _entity = GetComponentInParent<EntityController>();
        }

        public void CurrentStateAnimTrigger() => _entity.CallAnimTrigger();
    }
}