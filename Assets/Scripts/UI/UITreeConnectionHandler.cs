using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI
{
    [Serializable]
    public class UITreeConnectionDetails
    {
        public UITreeConnectionHandler childNode;
        public NodeDirectionType directionType;
        [Range(100f, 350f)] public float length;
        [Range(-25f, 25f)] public float rotation;
    }

    [RequireComponent(typeof(RectTransform))]
    public class UITreeConnectionHandler : MonoBehaviour
    {
        [Header("Connections Config")]
        [SerializeField] private UITreeConnectionDetails[] connectionDetails;
        [SerializeField] private UITreeConnection[] connections;

        private RectTransform rectTransform;
        private Image connectionImage;
        private Color originalColor;
#if UNITY_EDITOR
        private bool validationUpdateScheduled;
#endif

        private void Awake()
        {
            if (connectionImage) originalColor = connectionImage.color;
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (validationUpdateScheduled) return;

            validationUpdateScheduled = true;
            EditorApplication.delayCall += UpdateConnectionsAfterValidation;
#endif
        }

#if UNITY_EDITOR
        private void UpdateConnectionsAfterValidation()
        {
            validationUpdateScheduled = false;
            if (!this) return;

            UpdateConnections();
        }
#endif

        public void SetConnectionImage(Image image) => connectionImage = image;

        public void ConnectionImageUnlocked(bool isUnlocked)
        {
            if (!connectionImage) return;
            connectionImage.color = isUnlocked ? Color.white : originalColor;
        }

        private bool EnsureRectTransform()
        {
            if (!rectTransform) rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }

        private void SetPosition(Vector3 position)
        {
            if (!EnsureRectTransform())
            {
                Debug.LogError("UITreeConnectionHandler requires a RectTransform. - " + gameObject.name, this);
                return;
            }

            rectTransform.localPosition = position;
        }

        public void UpdateConnections()
        {
            if (!EnsureRectTransform())
            {
                Debug.LogError("UITreeConnectionHandler requires a RectTransform. - " + gameObject.name, this);
                return;
            }

            if (connectionDetails == null || connections == null)
            {
                Debug.LogWarning("Connections and connection details must be assigned. - " + gameObject.name, this);
                return;
            }

            if (connectionDetails.Length != connections.Length)
            {
                Debug.LogWarning("Number of connections and details must be the same. - " + gameObject.name, this);
                return;
            }

            for (int i = 0; i < connectionDetails.Length; i++)
            {
                UITreeConnectionDetails detail = connectionDetails[i];
                UITreeConnection connection = connections[i];

                if (detail == null || !connection)
                {
                    Debug.LogWarning("Connection details and connections must not contain empty entries. - " + gameObject.name, this);
                    continue;
                }
                // could be an issue with the node child conn point pos
                Vector2 targetPosition = connection.GetConnectionPointPosition(rectTransform);
                Image connImg = connection.GetConnectionLenghtImage();

                connection.DirectConnection(detail.directionType, detail.length, detail.rotation);

                if (detail.childNode == null) continue;

                detail.childNode.SetPosition(targetPosition);
                detail.childNode.SetConnectionImage(connImg);
                detail.childNode.transform.SetAsLastSibling();
            }
        }

        public void UpdateAllConnections()
        {
            UpdateConnections();

            if (connectionDetails == null) return;

            foreach (UITreeConnectionDetails node in connectionDetails)
            {
                if (node == null || !node.childNode) continue;
                node.childNode.UpdateConnections();
            }
        }
    }
}