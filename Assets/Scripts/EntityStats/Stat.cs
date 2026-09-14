using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityStats
{
    [Serializable]
    public class Stat
    { 
        [SerializeField] private float baseValue;
        public float GetValue => baseValue;
    }
}