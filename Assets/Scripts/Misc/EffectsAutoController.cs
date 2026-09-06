using UnityEngine;

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
        [Space]
        [SerializeField] private float xMinOffset = -0.3f;
        [SerializeField] private float xMaxOffset = 0.3f;
        [Space]
        [SerializeField] private float yMinOffset = -0.3f;
        [SerializeField] private float yMaxOffset = 0.3f;
        

        private void Start()
        {
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
            
            float zRotation = Random.Range(0, 360);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }
}
