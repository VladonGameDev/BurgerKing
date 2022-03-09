using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private int numOfCalls = 0;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake()
    {
        numOfCalls = 0;
        Invoke("CamRotationPlus", 0f);
    }

    private void CamRotationPlus()
    {
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.x = 0.15f;
        numOfCalls++;
        Invoke("CamRotationZero", 0.075f);
    }
    private void CamRotationZero()
    {
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.x = 0;
        numOfCalls++;
        if(numOfCalls < 7)
            Invoke("CamRotationMinus", 0.075f);
    }
    private void CamRotationMinus()
    {
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset.x = -0.15f;
        numOfCalls++;
        Invoke("CamRotationZero", 0.075f);
    }
}
