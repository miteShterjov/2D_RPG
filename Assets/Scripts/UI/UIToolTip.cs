using UnityEngine;

namespace UI
{
    public class UIToolTip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Vector2 toolTipOffset = new Vector2(300f, 20f);
        
        private RectTransform rectTransform;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void ShowToolTip(bool showToolTip, RectTransform targetRectTransform)
        {
            if (!showToolTip)
            {
                rectTransform.position = new Vector2(9999, 9999);
                return;
            }
            UpdatePosition(targetRectTransform);
        }

        private void UpdatePosition(RectTransform targetRectTransform)
        {
            float screenTop = Screen.height;
            float screenBottom = 0f;
            
            float screenCenterX = Screen.width / 2f;
            Vector2 targetPosition = targetRectTransform.position;
            targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - toolTipOffset.x : targetPosition.x + toolTipOffset.x;
            
            float tooltipHalf = rectTransform.sizeDelta.y / 2f;
            float topY = targetPosition.y + tooltipHalf;
            float bottomY = targetPosition.y - tooltipHalf;
            
            if (topY > screenTop) targetPosition.y = screenTop - tooltipHalf - toolTipOffset.y;
            if (bottomY < screenBottom) targetPosition.y = screenBottom + tooltipHalf + toolTipOffset.y;
            
            this.rectTransform.position = targetPosition;
        }
    }
}