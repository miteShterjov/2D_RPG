using System.Collections;
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
        [SerializeField] private GameObject onHitEffectPrefab;
    
        private Material originalMaterial;                                                                                            
        private SpriteRenderer spriteRenderer;
    
        private Coroutine onDamageVFXCoroutine;
        private Coroutine knockbackVFXCoroutine;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null) originalMaterial = spriteRenderer.material;
        }

        public void PlayOnHitEffect(Transform target)
        {
            if (onHitEffectPrefab == null || target == null) return;

            GameObject onHitEffect = Instantiate(onHitEffectPrefab, target.position, Quaternion.identity);
            SpriteRenderer effectRenderer = onHitEffect.GetComponentInChildren<SpriteRenderer>();
            if (effectRenderer != null) effectRenderer.color = onHitEffectColor;
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

        private IEnumerator KnockBackVFXCo(int direction)
        {
            EntityMoveController entityMove = GetComponent<EntityMoveController>();
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            if (entityMove == null || rigidbody == null) yield break;

            entityMove.IsKnockedBack = true;
            rigidbody.linearVelocity = knockbackForce * direction;
        
            yield return new WaitForSeconds(knockbackDuration);
        
            entityMove.IsKnockedBack = false;
            rigidbody.linearVelocity = Vector2.zero;
            knockbackVFXCoroutine = null;
        }
    }
}
