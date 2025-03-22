using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystem : MonoBehaviour {

    public static CameraSystem Instance {
        private set;
        get;
    }

    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float orbitSpeed = 0.1f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float focusSpeed = 8f;

    [SerializeField] private CinemachineOrbitalFollow orbitCamera;

    private CameraInput_Actions cameraInputActions;
    private bool isShiftHeld = false;
    private bool isMiddleMouseHeld = false;
    private Vector2 moveInput;

    public Vector3 targetPosition;
    private bool centeredOnTarget;
    private bool focusing = false;

    //private CinemachineOrbitalFollow orbitalFollowComponent;

    private const float minPitch = 10f;
    private const float maxPitch = 85f;

    private void Awake() {
        Instance = this;
        cameraInputActions = new CameraInput_Actions();
        //orbitalFollowComponent = orbitCamera.GetComponent<CinemachineOrbitalFollow>();

        cameraInputActions.Camera.Move.started += ctx => isShiftHeld = true;
        cameraInputActions.Camera.Move.canceled += ctx => isShiftHeld = false;

        // Detect Middle Mouse Button
        cameraInputActions.Camera.MiddleMouse.started += ctx => isMiddleMouseHeld = true;
        cameraInputActions.Camera.MiddleMouse.canceled += ctx => isMiddleMouseHeld = false;

        // Track mouse movement (only if both Shift & Middle Mouse are held)
        cameraInputActions.Camera.MoveMouse.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        cameraInputActions.Camera.MoveMouse.canceled += ctx => moveInput = Vector2.zero;

        cameraInputActions.Camera.ZoomIn.performed += ctx => ZoomIn();
        cameraInputActions.Camera.ZoomOut.performed += ctx => ZoomOut();

        cameraInputActions.Camera.Focus.performed += ctx => focusing = true;

        cameraInputActions.Camera.Enable();
    }

    private void OnEnable() => cameraInputActions.Enable();
    private void OnDisable() => cameraInputActions.Disable();

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            focusing = false;
            centeredOnTarget = true;
        }
        else { centeredOnTarget = false; }

        if (!centeredOnTarget && focusing)
        {
            Focus();
        }

        if (isShiftHeld && isMiddleMouseHeld)
        {
            MoveHorizontal();
        
        } else if (isMiddleMouseHeld)
        {
            OrbitCamera();
        }


    }

    private void MoveHorizontal()
    {
        Vector3 forward = orbitCamera.transform.forward;
        Vector3 right = orbitCamera.transform.right;

        // Ignore vertical movement (Y-axis) to keep it on a horizontal plane
        forward.y = 0;
        right.y = 0;

        // Normalize to avoid diagonal movement being faster
        forward.Normalize();
        right.Normalize();

        // Move in the direction of mouse input
        Vector3 moveDirection = (right * moveInput.x + forward * moveInput.y) * .1f * moveSpeed;

        // Apply movement
        transform.position -= moveDirection;
    }

    private void OrbitCamera()
    {
        float yaw = moveInput.x * orbitSpeed;
        float pitch = -moveInput.y * orbitSpeed;

        orbitCamera.HorizontalAxis.Value += yaw;

        // Apply Pitch with clamping (limits between 10° and 85°)
        float newPitch = orbitCamera.VerticalAxis.Value + pitch;
        orbitCamera.VerticalAxis.Value = Mathf.Clamp(newPitch, minPitch, maxPitch);
    }

    private void ZoomIn()
    {
        if (orbitCamera.Radius + zoomSpeed > 20f) { 
            orbitCamera.Radius -= zoomSpeed;
        }
    }
    private void ZoomOut()
    {
        orbitCamera.Radius += zoomSpeed;
    }

    private void Focus()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, focusSpeed * Time.deltaTime);
    }
}
