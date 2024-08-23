using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ScrollSnap : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public RectTransform referenceTransform; // The reference transform for snapping
        public float snapSpeed = 10f;

        private RectTransform contentRect;
        private bool isSnapping = false;
        private Vector2 targetPosition;

        void Start()
        {
            contentRect = scrollRect.content;

            // If referenceTransform is not assigned, default to the viewport
            if (referenceTransform == null)
            {
                referenceTransform = scrollRect.viewport;
            }
        }

        void Update()
        {
            if (isSnapping)
            {
                contentRect.anchoredPosition = Vector2.Lerp(contentRect.anchoredPosition, targetPosition, snapSpeed * Time.deltaTime);

                // Stop snapping when close enough to the target
                if (Vector2.Distance(contentRect.anchoredPosition, targetPosition) < 0.1f)
                {
                    contentRect.anchoredPosition = targetPosition;
                    isSnapping = false;
                }
            }
        }

        public void SnapToClosest()
        {
            float minDistance = float.MaxValue;
            RectTransform closestChild = null;

            // Get the reference position in local space of the content
            Vector2 referenceLocalPosition = referenceTransform.localPosition;

            // Find the closest child to the referenceTransform's position
            foreach (RectTransform child in contentRect)
            {
                Vector2 childLocalPosition = child.localPosition;
                float distance = Vector2.Distance(childLocalPosition, referenceLocalPosition);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestChild = child;
                }
            }

            if (closestChild != null)
            {
                // Calculate the target position to snap to
                Vector2 closestChildLocalPosition = closestChild.localPosition;
                targetPosition = contentRect.anchoredPosition + (referenceLocalPosition - closestChildLocalPosition);

                isSnapping = true;
            }
        }

        public void OnSectionClosed()
        {
            SnapToClosest(); // Call this when closing the section
        }
    }
}
