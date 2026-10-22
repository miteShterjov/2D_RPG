using System.Collections;
using EntityControl;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerVFX : EntityVFX
    {
        [Header("Image Echo Settings")] 
        [Range(.01f, .02f)] 
        [SerializeField] private float imageEchoInterval = .05f;
        [SerializeField] private GameObject imageEchoPrefab;

        private Coroutine imageEchoCo;

        public void DoImageEchoEffect(float duration)
        {
            if (imageEchoCo != null) StopCoroutine(imageEchoCo);
            imageEchoCo = StartCoroutine(ImageEchoEffect(duration));
        }

        private IEnumerator ImageEchoEffect(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                CreateImageEcho();
                
                yield return new WaitForSeconds(imageEchoInterval);
                time += imageEchoInterval;
            }
            
            Destroy(imageEchoPrefab);
        }

        private void CreateImageEcho()
        {
            GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, Quaternion.identity);
            imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = spriteRenderer.sprite;
        }

    }
}