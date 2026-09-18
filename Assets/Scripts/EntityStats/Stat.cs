using System;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStats
{
    [Serializable]
    public class Stat
    { 
        [SerializeField] private float baseValue;
        [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();
        
        private float finalValue;
        private bool needsRecalculation = true;

        public float GetValue()
        {
            if (!needsRecalculation) return finalValue;
            needsRecalculation = false;
            finalValue = GetFinalValue();
            return finalValue;
        }
        
        private float GetFinalValue()
        {
            finalValue = baseValue;
            foreach (StatModifier modifier in modifiers) finalValue += modifier.value;
            return finalValue;
        }
        
        public void AddModifier(StatModifier modifier)
        {
            modifiers.Add(modifier);
            needsRecalculation = true;
        }
        
        public void AddModifier(string source, float value, float duration)
        {
            modifiers.Add(new StatModifier(value, duration, source));
            needsRecalculation = true;
        }
        
        public void RemoveModifier(StatModifier modifier)
        {
            modifiers.Remove(modifier);
            needsRecalculation = true;
        }
        
        public void RemoveModifier(string source)
        {
            modifiers.RemoveAll(m => m.source == source);
            needsRecalculation = true;
        } 
    }

    [Serializable]
    public class StatModifier
    {
        public float value;
        public float duration;
        public string source;
        
        public StatModifier(float value, float duration, string source)
        {
            this.value = value;
            this.duration = duration;
            this.source = source;
        }
    }
}