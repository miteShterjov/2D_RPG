using UnityEngine;

namespace EntityControl
{
    public class EntityAnimTriggersHandler : MonoBehaviour
    {
        private EntityController _entity;
        private EntityCombatController _entityCombat;

        protected virtual void Awake()
        {
            _entity = GetComponentInParent<EntityController>();
            _entityCombat = GetComponentInParent<EntityCombatController>();
        }

        public void CurrentStateAnimTrigger() => _entity.CallAnimTrigger();

        public void AttackTrigger() => _entityCombat.PreformAttackEffect();
    }
}