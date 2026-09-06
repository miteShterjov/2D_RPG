using System;
using Blueprints;
using UnityEngine;

namespace EntityControl
{
    public class EntityController : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public Rigidbody2D Rb => entityMove.rb;

        public EntityMoveController entityMove;
        public EntityCollisionController entityCollision;
        
        protected StateMachine _stateMachine;

        protected virtual void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            
            entityMove = GetComponent<EntityMoveController>();
            entityCollision = GetComponent<EntityCollisionController>();
        }
        
        protected virtual void Update() => print("Current state of " + gameObject.tag + ": " + _stateMachine.CurrentState.GetType());
        
        public void CallAnimTrigger() => _stateMachine.CurrentState.CallAnimTrigger();

        public virtual void EntityDeath()
        {
        }
    }
}