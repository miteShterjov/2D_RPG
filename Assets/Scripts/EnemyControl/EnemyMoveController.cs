using System;
using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyMoveController : EntityMoveController
    {
        private static readonly int MoveAnimSpeedMultyplier = Animator.StringToHash(moveAnimMultiplier);

        [Header("Locomotion Config")] [SerializeField]
        public float patrolIdleTime = 1.5f;
        [SerializeField] [Range(0, 2)] public float moveAnimSpeedMultyplier;

        protected const string moveAnimMultiplier = "moveAnimSpeedMultyplier";
        
        protected EnemyController enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyController>();
        }

        protected void Update()
        {
            enemy.Animator.SetFloat(MoveAnimSpeedMultyplier, moveAnimSpeedMultyplier);
        }
    }
}