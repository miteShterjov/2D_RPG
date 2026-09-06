using System;
using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyMoveController : EntityMoveController
    {
        

        [Header("Locomotion Config")] 
        [SerializeField] public float patrolIdleTime = 1.5f;
        [SerializeField] [Range(0, 2)] public float moveAnimSpeedMultyplier;

        protected EnemyController enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyController>();
        }
    }
}