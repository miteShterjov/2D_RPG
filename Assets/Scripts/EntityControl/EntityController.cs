using EntityStateMachine;
using EntityStats;
using UnityEngine;

namespace EntityControl
{
    public class EntityController : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public Rigidbody2D Rb => entityMove.rb;
        public GeneralStats generalStats;
         
        public EntityMoveController entityMove;
        public EntityCollisionController entityCollision;
        
        protected StateMachine StateMachine;

        protected virtual void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            
            entityMove = GetComponent<EntityMoveController>();
            entityCollision = GetComponent<EntityCollisionController>();
            generalStats = GetComponent<GeneralStats>();
        }
        
        protected virtual void Update()
        {
        }
        
        public void CallAnimTrigger() => StateMachine?.CurrentState?.CallAnimTrigger();

        public virtual void EntityDeath()
        {
        }
    }
}