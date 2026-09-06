using System.Collections;
using EntityControl;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
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
        originalMaterial = spriteRenderer.material;
    }

    public void PlayOnHitEffect(Transform target)
    {
        GameObject onHitEffect = Instantiate(onHitEffectPrefab, target.position, Quaternion.identity);
        onHitEffect.GetComponentInChildren<SpriteRenderer>().color = onHitEffectColor;
    }
    
    public void PlayOnDamageFlashVFX()
    {
        if (onDamageVFXCoroutine != null) StopCoroutine(onDamageVFXCoroutine);
        StartCoroutine(OnDamageVFXCo());
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
    }

    private IEnumerator KnockBackVFXCo(int direction)
    {
        GetComponent<EntityMoveController>().IsKnockedBack = true;
        GetComponent<Rigidbody2D>().linearVelocity = knockbackForce * direction;
        
        yield return new WaitForSeconds(knockbackDuration);
        
        GetComponent<EntityMoveController>().IsKnockedBack = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
    
}
