using Unity.Cinemachine;
using UnityEngine;

public class OrbitCameraSpeedController : MonoBehaviour
{
    [SerializeField] CinemachineOrbitalFollow orbitCamera;
    [SerializeField] private float orbitSpeed =1f;

    private void Update()
    {
        if (orbitCamera != null)
        {
            orbitCamera.HorizontalAxis.Value += orbitSpeed * Time.deltaTime;
        }
    }

}
