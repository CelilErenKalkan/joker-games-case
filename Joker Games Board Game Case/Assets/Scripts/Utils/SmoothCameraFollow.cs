using System.Collections.Generic;
using Game_Management;
using UnityEngine;

namespace Utils
{
    public class SmoothCameraFollow : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public float smoothSpeed = 0.01f;
        [HideInInspector] public Vector3 offset;
        [HideInInspector] public float rotationSpeed = 10f; // Speed of rotation around the grids
        [HideInInspector] public float transitionSpeed = 2f; // Speed of transition when a target appears
        [HideInInspector] public float maxRotationAngle = 360f; // Maximum rotation around grids

        private Quaternion initialRotation;
        private bool rotatingAroundGrids = false;
        private GameManager _gameManager;

        private void Start()
        {
            initialRotation = transform.rotation; // Store the initial rotation
            _gameManager = GameManager.Instance;
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                FollowTarget();
            }
            else
            {
                HandleNoTarget();
            }
        }

        private void FollowTarget()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // Smoothly transition back to the initial rotation when following a target
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * transitionSpeed);
        }

        private void HandleNoTarget()
        {
            // Code to ensure all grids are visible from a distance and rotate around the grids
            List<Transform> gridTransforms = GetGridTransforms(); // Assume this method provides the list of grid transforms
            if (gridTransforms == null || gridTransforms.Count == 0) return;

            Bounds gridBounds = GetBounds(gridTransforms);
            Vector3 centerPosition = gridBounds.center;

            float distance = CalculateDistanceToCoverGrid(gridBounds);
            Vector3 desiredPosition = centerPosition - transform.forward * distance;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Start rotating around the grids
            rotatingAroundGrids = true;
            RotateAroundGrids(centerPosition);
        }

        private void RotateAroundGrids(Vector3 centerPosition)
        {
            if (rotatingAroundGrids)
            {
                transform.RotateAround(centerPosition, Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }

        private Bounds GetBounds(List<Transform> transforms)
        {
            Bounds bounds = new Bounds(transforms[0].position, Vector3.zero);
            foreach (Transform t in transforms)
            {
                bounds.Encapsulate(t.position);
            }
            return bounds;
        }

        private float CalculateDistanceToCoverGrid(Bounds bounds)
        {
            float cameraFOV = Camera.main.fieldOfView;
            float cameraAspect = Camera.main.aspect;

            float gridHeight = bounds.size.y;
            float gridWidth = bounds.size.x;

            float distanceY = gridHeight / (2f * Mathf.Tan(Mathf.Deg2Rad * cameraFOV / 2f));
            float distanceX = gridWidth / (2f * Mathf.Tan(Mathf.Deg2Rad * cameraFOV / 2f) * cameraAspect);

            return Mathf.Max(distanceY, distanceX);
        }

        // Placeholder for your method to get grid transforms
        private List<Transform> GetGridTransforms()
        {
            // Implement this method to return the list of grid transforms
            return _gameManager.gameMap;
        }
    }
}
