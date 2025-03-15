using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystem : MonoBehaviour {

    public static CameraSystem Instance {
        private set;
        get;
    }

    private PlayerInput playerInput;
    private CameraInput_Actions cameraInputActions;

    private void Awake() {
        Instance = this;
        cameraInputActions = new CameraInput_Actions();
        cameraInputActions.Camera.Enable();
    }

    private void Update() {

        float rotateDirY = cameraInputActions.Camera.Rotation.ReadValue<Vector2>().x;
        float rotateDirX = cameraInputActions.Camera.Rotation.ReadValue<Vector2>().y;

        float rotateSpeed = 100f;

        transform.eulerAngles += new Vector3(0, -rotateDirY * rotateSpeed * Time.deltaTime, 0);

        Vector3 moveDir = cameraInputActions.Camera.Movement.ReadValue<Vector3>();

        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }

    public void DisableInput() {
        cameraInputActions.Disable();
    }

    public void EnableInput() {
        cameraInputActions.Enable();
    }
}
