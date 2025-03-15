using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraChangeController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camA;
    [SerializeField] private CinemachineCamera camB;

    private bool isCamAActive = true;

    public void Transition()
    {
        if (isCamAActive)
        {
            camA.Priority = 5;
            camB.Priority = 10;
        }
        else
        {
            camA.Priority = 10;
            camB.Priority = 5;
        }

        isCamAActive = !isCamAActive;
    }
}
