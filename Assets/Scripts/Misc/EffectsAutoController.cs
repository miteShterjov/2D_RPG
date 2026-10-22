using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    public class EffectsAutoController : MonoBehaviour
    {
        [Header("Auto Destroy Config")] 
        [SerializeField] private bool autoDestroy = true;
        [SerializeField] private float destroyDelay = 0.8f;
        [Header("Random Offset Config")]
        [SerializeField] private bool randomOffset = true;
        [SerializeField] private bool randomRotation = true;
        [Header("Random Rotation Range")]
        [SerializeField] private float minRotation = 0f;
        [SerializeField] private float maxRotation = 360f;
        [Space]
        [Header("Random Offset Range")]
        [SerializeField] private float xMinOffset = -0.3f;
        [SerializeField] private float xMaxOffset = 0.3f;
        [Space]
        [SerializeField] private float yMinOffset = -0.3f;
        [SerializeField] private float yMaxOffset = 0.3f;
        [Header("Fade Effect")]
        [SerializeField] private bool canFade;
        [SerializeField] private float fadeSpeed = 1f;
        
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (canFade) StartCoroutine(FadeEffectCo());
            
            ApplyRandomOffset();
            ApplyRandomRotation();
            
            if (autoDestroy) Destroy(gameObject, destroyDelay);
        }

        private void ApplyRandomOffset()
        {
            if (!randomOffset) return;
            
            float xOffset = Random.Range(xMinOffset, xMaxOffset);
            float yOffset = Random.Range(yMinOffset, yMaxOffset);

            transform.position += new Vector3(xOffset, yOffset, 0f);
        }

        private void ApplyRandomRotation()
        {
            if (!randomRotation) return;
            
            float zRotation = Random.Range(minRotation, maxRotation);
            transform.Rotate(0f, 0f, zRotation);
        }

        private IEnumerator FadeEffectCo()
        {
            Color targetColor = Color.white;
            
            while (targetColor.a > 0)
            {
                targetColor.a -= fadeSpeed * Time.deltaTime;
                spriteRenderer.color = targetColor;
                yield return null;
            }

            spriteRenderer.color = targetColor;
        }
    }
}
