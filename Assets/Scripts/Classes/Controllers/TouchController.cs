using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dreamteck.Splines;

public class TouchController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Transform playerObj;

    public GameData gameData;
    private float ShiftRange, ShiftModifier;
    private UIManager UIManager;
    private SplineFollower splineFollower;
    private StackController stackController;
    private bool isGameStarted;



    private int Index;
    private PointerEventData EventData;


    void Awake()
    {
        stackController = GameObject.Find("PlayerObj").GetComponent<StackController>();
        playerObj = GameObject.Find("PlayerObj").transform;
        UIManager = GameObject.Find("[UI]").GetComponent<UIManager>();
        ShiftModifier = gameData.shiftModifier;
        ShiftRange = gameData.shiftRange;
        splineFollower = GameObject.Find("Player").GetComponent<SplineFollower>();
        splineFollower.followSpeed = 0;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    private void FixedUpdate()
    {
        //Проверки при переходе на следующий уровень, так как при преходе методы эвейк и старт не работают, из-за не ясных особенностей левел менеджера
        if(stackController == null)
        {
            stackController = GameObject.Find("PlayerObj").GetComponent<StackController>();
        }
        if(!stackController.enabled)
        {
            stackController.enabled = true;
        }
        if(splineFollower == null)
        {
            splineFollower = GameObject.Find("Player").GetComponent<SplineFollower>();
        }




        if (UIManager.isGameStarted)
        {
            splineFollower.followSpeed = 4;
        }
        if(!UIManager.isGameStarted)
        {
            splineFollower.followSpeed = 0;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        playerObj = GameObject.Find("PlayerObj").transform;
        if (UIManager.isGameStarted)
        {
            if (eventData.delta.x > 0)
            {
                if (playerObj.localPosition.x < ShiftRange)
                {
                    playerObj.localPosition += Vector3.right * ShiftModifier * eventData.delta.x;
                }
            }
            else if (eventData.delta.x < 0)
            {
                if (playerObj.localPosition.x > -ShiftRange)
                {
                    playerObj.localPosition += Vector3.left * ShiftModifier * (-eventData.delta.x);
                }
            }
        }
    }
}
