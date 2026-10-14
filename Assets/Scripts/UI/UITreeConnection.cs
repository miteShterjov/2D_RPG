using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum NodeDirectionType { None, UpLeft, UpRight, DownLeft, DownRight, Up, Down, Left, Right}
    
    public class UITreeConnection : MonoBehaviour
    {
        [Header("Connections Points")]
        [SerializeField] private RectTransform connectionPoint;
        [SerializeField] private RectTransform connectionLenght;
        [SerializeField] private RectTransform childNodeConnPoint;

        public void DirectConnection(NodeDirectionType direction, float length, float offset)
        {
            bool shouldBeActive = direction != NodeDirectionType.None;
            float finalLenght = shouldBeActive ?  length : 0;
            float angle = GetDirectionAngle(direction);
            
            connectionPoint.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
            connectionLenght.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalLenght);
        }

        public Vector3 GetConnectionPointPosition(RectTransform rect)
        {
            return rect.parent.InverseTransformPoint(childNodeConnPoint.position);
        }
        
        public Image GetConnectionLenghtImage() => connectionLenght.GetComponent<Image>();

        private static float GetDirectionAngle(NodeDirectionType directionType)
        {
            return directionType switch
            {
                NodeDirectionType.UpLeft => 135f,
                NodeDirectionType.UpRight => 45,
                NodeDirectionType.DownLeft => -135f,
                NodeDirectionType.DownRight => -45f,
                NodeDirectionType.Up => 90f,
                NodeDirectionType.Down => -90f,
                NodeDirectionType.Left => 180f,
                NodeDirectionType.Right => 0f,
                _ => 0
            };
        }
    }
}