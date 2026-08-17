using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Moves the player's view over the level. In this tower-defense game the
    /// camera and build selection together act as the player controller.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 12f;
        [SerializeField] private float moveSmoothTime = 0.12f;

        [Header("Movement Limits")]
        [SerializeField] private bool useMovementLimits = true;
        [SerializeField] private Vector2 horizontalLimits = new Vector2(-7f, 7f);
        [SerializeField] private Vector2 depthLimits = new Vector2(-23f, -15f);

        [Header("Zoom")]
        [SerializeField] private float zoomSensitivity = 0.05f;
        [SerializeField] private float zoomSmoothTime = 0.08f;
        [SerializeField] private Vector2 fieldOfViewLimits = new Vector2(38f, 70f);

        private Camera controlledCamera;
        private Vector3 targetPosition;
        private Vector3 moveVelocity;
        private float targetFieldOfView;
        private float zoomVelocity;

        private void Awake()
        {
            controlledCamera = GetComponent<Camera>();
            targetPosition = transform.position;
            targetFieldOfView = controlledCamera.fieldOfView;
        }

        private void Update()
        {
            ReadMovement();
            ReadZoom();
            ApplyMovement();
            ApplyZoom();
        }

        private void ReadMovement()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            float horizontal = 0f;
            float vertical = 0f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal -= 1f;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal += 1f;
            }
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                vertical -= 1f;
            }
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                vertical += 1f;
            }

            Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;
            targetPosition += input * moveSpeed * Time.unscaledDeltaTime;

            if (useMovementLimits)
            {
                targetPosition.x = Mathf.Clamp(
                    targetPosition.x, horizontalLimits.x, horizontalLimits.y);
                targetPosition.z = Mathf.Clamp(
                    targetPosition.z, depthLimits.x, depthLimits.y);
            }
        }

        private void ReadZoom()
        {
            if (Mouse.current == null)
            {
                return;
            }

            float scroll = Mouse.current.scroll.ReadValue().y;
            targetFieldOfView -= scroll * zoomSensitivity;
            targetFieldOfView = Mathf.Clamp(
                targetFieldOfView, fieldOfViewLimits.x, fieldOfViewLimits.y);
        }

        private void ApplyMovement()
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref moveVelocity,
                moveSmoothTime,
                Mathf.Infinity,
                Time.unscaledDeltaTime);
        }

        private void ApplyZoom()
        {
            controlledCamera.fieldOfView = Mathf.SmoothDamp(
                controlledCamera.fieldOfView,
                targetFieldOfView,
                ref zoomVelocity,
                zoomSmoothTime,
                Mathf.Infinity,
                Time.unscaledDeltaTime);
        }
    }
}
