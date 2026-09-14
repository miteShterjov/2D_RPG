using EntityControl;
using UnityEngine;

namespace UI
{
    public class UIMiniHealthBar : MonoBehaviour
    {
        private EntityMoveController entityMove;
        private Vector3 initialLocalScale;

        private void Awake()
        {
            entityMove = GetComponentInParent<EntityMoveController>();
            initialLocalScale = transform.localScale;
        }
        
        private void OnEnable() => entityMove.OnFlip += HandleFlip;
        private void OnDisable() => entityMove.OnFlip -= HandleFlip;

        private void LateUpdate() => transform.rotation = Quaternion.identity;
        
        private void HandleFlip(int dir) => transform.localScale = new Vector3(initialLocalScale.x * dir, initialLocalScale.y, initialLocalScale.z);
        
        
    }
}
