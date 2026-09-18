using System.Collections;
using EntityStats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects
{
    public class ObjectBuff : MonoBehaviour
    {
        [Header("Buff Config")]
        [SerializeField] private Buff[] buffs;
        [Space]
        [SerializeField] private string buffName;
        [SerializeField] private float buffDuration = 5f;
        [SerializeField] private bool canBeUsed = true;
        [Header("Float Config")]
        [SerializeField] private float floatSpeed = 1f;
        [SerializeField] private float floatRange = .1f;

        private GeneralStats statsToModify;
        
        private Vector3 startingPosition;
        private SpriteRenderer spriteRenderer;
        
        private void Awake()
        {
            startingPosition = transform.position;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        private void Update()
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            transform.position =  startingPosition + new Vector3(0, yOffset, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!canBeUsed) return;
            statsToModify = other.GetComponent<GeneralStats>();
            StartCoroutine(BuffCo());
        }

        private IEnumerator BuffCo()
        {
            canBeUsed = false;
            spriteRenderer.color = Color.clear;

            ApplyBuff(true);
            yield return new WaitForSeconds(buffDuration);
            ApplyBuff(false);
            
            Destroy(gameObject);
        }

        private void ApplyBuff(bool apply)
        {
            foreach (Buff buff in buffs)
            {
                if (apply) statsToModify.GetStatByType(buff.type).AddModifier(buffName, buff.value, buffDuration);
                else statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
            }
        }
    }

    [System.Serializable]
    public class Buff
    {
        public StatType type;
        public float value;
    }
}