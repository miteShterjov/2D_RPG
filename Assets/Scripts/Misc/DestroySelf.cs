using UnityEngine;

namespace Misc
{   
    public class DestroySelf : MonoBehaviour
    {
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private bool destroyOnStart = true;
        [SerializeField] GameObject explosionEffectPrefab;

        private void Start()
        {
            if (destroyOnStart) Destroy(gameObject, lifetime);
            Invoke(nameof(OnDestroy), lifetime - 0.1f);
        }

        public void DestroyNow() => Destroy(gameObject);

        private void OnDestroy()
        {
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }
        }

        //todo: at time of destroy if shard in collision with enemy, deal damage to enemy
    }
}

