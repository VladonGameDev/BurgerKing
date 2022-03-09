using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{
    private Cinemachine3rdPersonFollow personFollow;
    private StackController stackController;
    private float moveDistance, previousCamDistance;
    private bool move = false;
    private void Awake()
    {
        personFollow = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        stackController = GameObject.Find("PlayerObj").GetComponent<StackController>();
    }
    
    public void UpdateCamDistance(int futureBurgerCount, int currentBurgerCount)
    {
        if(currentBurgerCount < futureBurgerCount)
        {
            moveDistance = (futureBurgerCount - currentBurgerCount) * 0.1f;
            previousCamDistance = personFollow.CameraDistance;
            move = true;
        }
    }
    
    void FixedUpdate()
    {
        if(personFollow.CameraDistance < previousCamDistance + moveDistance && move)
        {
            personFollow.CameraDistance += 0.01f;
            personFollow.VerticalArmLength += 0.005f;
        }
        else
        {
            move = false;
        }
    }
}
