using System.Collections;
using EntityStats;
using UnityEngine;

namespace EntityControl
{
    public class EntityVFX : MonoBehaviour
    {
        [Header("On Damage VFX")]
        [SerializeField] private Material onDamageMaterial;
        [SerializeField] private float onDamageDuration = 0.2f;
        [Header("Knockback VFX")]
        [SerializeField] private Vector2 knockbackForce = new Vector2(1.5f, 2.5f);
        [SerializeField] private float knockbackDuration = 0.2f;
        [Header("OnHitEffect")]
        [SerializeField] private Color onHitEffectColor = Color.white;
        [SerializeField] private GameObject onHitPrefab;
        [SerializeField] private GameObject critPrefab;
        [Header("Ele hitEffect colors")]
        [SerializeField] private Color fireHitEffectColor = Color.orangeRed;
        [SerializeField] private Color iceHitEffectColor = Color.cyan;
        [SerializeField] private Color thunderHitEffectColor = Color.darkGray;
    
        private Material originalMaterial;                                                                                            
        private SpriteRenderer spriteRenderer;
        
        private EntityMoveController entityMove;
    
        private Coroutine onDamageVFXCoroutine;
        private Coroutine knockbackVFXCoroutine;
        
        private Color originalColor;

        protected virtual void Awake()
        {
            entityMove = GetComponent<EntityMoveController>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null) originalMaterial = spriteRenderer.material;
            
            originalColor = onHitEffectColor;
        }

        public void PlayOnHitEffect(Transform target, bool isCrit)
        {
            if (!onHitPrefab) return;
            if (!critPrefab) return;
            
            GameObject hitPrefab = isCrit ? critPrefab : onHitPrefab;

            GameObject onHitEffect = Instantiate(hitPrefab, target.position, Quaternion.identity);
            SpriteRenderer effectRenderer = onHitEffect.GetComponentInChildren<SpriteRenderer>();
            if (effectRenderer) effectRenderer.color = onHitEffectColor;
            if (entityMove != null && entityMove.FacingDir < 0 && isCrit)
                onHitEffect.transform.Rotate(0, 180, 0);
        }
    
        public void PlayOnDamageFlashVFX()
        {
            if (spriteRenderer == null || onDamageMaterial == null) return;
            if (onDamageVFXCoroutine != null) StopCoroutine(onDamageVFXCoroutine);
            onDamageVFXCoroutine = StartCoroutine(OnDamageVFXCo());
        }
    
        public void PlayKnockBackVFX(int direction)  
        {
            if (knockbackVFXCoroutine != null) StopCoroutine(knockbackVFXCoroutine);
            knockbackVFXCoroutine = StartCoroutine(KnockBackVFXCo(direction));
        }

        private IEnumerator OnDamageVFXCo()
        {
            spriteRenderer.material = onDamageMaterial;
            yield return new WaitForSeconds(onDamageDuration);
            spriteRenderer.material = originalMaterial;
            onDamageVFXCoroutine = null;
        }

        public void UpdateOnHitEffectColor(ElementType elementType)
        {
            onHitEffectColor = elementType switch
            {
                ElementType.Fire => fireHitEffectColor,
                ElementType.Ice => iceHitEffectColor,
                ElementType.Lightning => thunderHitEffectColor,
                ElementType.None => originalColor,
                _ => onHitEffectColor
            };
        }

        public void PlayOnStatusVFX(float duration, ElementType elementType)
        {
            if (elementType == ElementType.Ice) 
                StartCoroutine(PlayStatusVFX(iceHitEffectColor, duration));
        }

        private IEnumerator KnockBackVFXCo(int direction)
        {
            entityMove = GetComponent<EntityMoveController>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (entityMove == null || rb == null) yield break;

            entityMove.IsKnockedBack = true;
            rb.linearVelocity = knockbackForce * direction;
        
            yield return new WaitForSeconds(knockbackDuration);
        
            entityMove.IsKnockedBack = false;
            rb.linearVelocity = Vector2.zero;
            knockbackVFXCoroutine = null;
        }

        private IEnumerator PlayStatusVFX(Color color, float duration)
        {
            float tickInterval = 0.25f;
            float timer = 0;

            Color lighterColor = iceHitEffectColor * 1.25f;
            Color darkerColor = iceHitEffectColor * 0.75f;

            bool toggle = false;

            while (timer < duration)
            {
                spriteRenderer.color = toggle ? lighterColor : darkerColor;
                toggle = !toggle;
                yield return new WaitForSeconds(tickInterval);
                timer += tickInterval;
            }
            
            spriteRenderer.color = Color.white;
        }
    }
}
