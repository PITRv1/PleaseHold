using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystem : MonoBehaviour {

    public static CameraSystem Instance {
        private set;
        get;
    }

    public event EventHandler OnPauseKeyPressed;

    [SerializeField] private float moveSpeedMult = 0.1f;
    [SerializeField] private float orbitSpeedMult = 0.1f;
    [SerializeField] private float zoomSpeedMult = 1f;

    private float moveSpeed = 0.1f;
    private float orbitSpeed = 0.1f;
    private float zoomSpeed = 1f;
    private float focusSpeed = 8f;

    [SerializeField] private CinemachineOrbitalFollow orbitCamera;

    private CameraInput_Actions cameraInputActions;
    private bool isShiftHeld = false;
    private bool isMiddleMouseHeld = false;
    private Vector2 moveInput;

    public Vector3 targetPosition;
    private bool centeredOnTarget;
    private bool focusing = false;

    private const float minPitch = 10f;
    private const float maxPitch = 85f;

    private void Awake() {
        Instance = this;
        cameraInputActions = new CameraInput_Actions();

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
        
        cameraInputActions.UI.Pause.performed += ctx => OnPauseKeyPressed?.Invoke(this, EventArgs.Empty);

        cameraInputActions.Camera.Enable();
    }

    private void OnEnable() => cameraInputActions.Enable();
    private void OnDisable() => cameraInputActions.Disable();

    private void Start()
    {
        float moveSensPref = PlayerPrefs.GetFloat("CameraMoveSens");
        moveSpeed = moveSensPref * moveSpeedMult;

        float orbitSensPref = PlayerPrefs.GetFloat("CameraOrbitSens");
        orbitSpeed = orbitSensPref * orbitSpeedMult;

        float zoomSensPref = PlayerPrefs.GetFloat("CameraZoomSens");
        zoomSpeed = zoomSensPref * zoomSpeedMult;
    }

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

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        float moveSensPref = PlayerPrefs.GetFloat("CameraMoveSens");
        moveSpeed = moveSensPref * moveSpeedMult;

        Vector3 moveDirection = (right * moveInput.x + forward * moveInput.y) * .1f * moveSpeed;

        transform.position -= moveDirection;
    }

    private void OrbitCamera()
    {

        float orbitSensPref = PlayerPrefs.GetFloat("CameraOrbitSens");
        orbitSpeed = orbitSensPref * orbitSpeedMult;

        float yaw = moveInput.x * orbitSpeed;
        float pitch = -moveInput.y * orbitSpeed;

        orbitCamera.HorizontalAxis.Value += yaw;

        float newPitch = orbitCamera.VerticalAxis.Value + pitch;
        orbitCamera.VerticalAxis.Value = Mathf.Clamp(newPitch, minPitch, maxPitch);
    }

    private void ZoomIn()
    {
        float zoomSensPref = PlayerPrefs.GetFloat("CameraZoomSens");
        zoomSpeed = zoomSensPref * zoomSpeedMult;

        if (orbitCamera.Radius - zoomSpeed > 20f) { 
            orbitCamera.Radius -= zoomSpeed;
        }
    }
    private void ZoomOut()
    {
        float zoomSensPref = PlayerPrefs.GetFloat("CameraZoomSens");
        zoomSpeed = zoomSensPref * zoomSpeedMult;

        if (orbitCamera.Radius + zoomSpeed < 1000f)
            orbitCamera.Radius += zoomSpeed;
    }

    private void Focus()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, focusSpeed * Time.deltaTime);
    }

    public void DisableCamInputs() { cameraInputActions.Camera.Disable(); }
    public void EnableCamInputs() { cameraInputActions.Camera.Enable(); }

    public void DisableUIInputs() { cameraInputActions.UI.Disable(); }

    private void Signiture()
    {
        Debug.Log("undefined");
    }
}
