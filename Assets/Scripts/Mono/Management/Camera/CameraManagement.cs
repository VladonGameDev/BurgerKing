using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    public static CameraManagement Active { get; private set; }
    public CameraManagement()
    {
        Active = this;
    }
    
    public static Vector2 ScreenCenter { get => new Vector2(Screen.width, Screen.height) / 2; }

    public float ShakeRotation;


    public bool MoveLock { get; private set; }
    public static void LockCam(bool on)
    {
        Active.MoveLock = on;
    }

    private void Start() => Init();



    private void Init()
    {

    }

    #region Animation

    public void Shake(float Power)
    {
        if(ShakeCoroutine == null)
        {
            ShakeCoroutine = StartCoroutine(ShakeCour(Power));
        }
    }
    private Coroutine ShakeCoroutine;
    private IEnumerator ShakeCour(float Power)
    {
        float shakeTime = DataHolder.Data.ShakeTime * Power;
        float currantTime = 0;

        float Angle = 0;

        Quaternion prevRotation = transform.rotation;

        while(currantTime < shakeTime)
        {
            Angle = DataHolder.Data.ShakeCurve.Evaluate(currantTime / shakeTime) * DataHolder.Data.MaxShakeAngle * Power * Mathf.Deg2Rad;

            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, prevRotation.z + Angle, transform.rotation.w);


            currantTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        transform.rotation = prevRotation;

        ShakeCoroutine = null;
        yield break;
    }

    #endregion

    #region Check

    public static bool AboveScreen(Vector3 point)
    {
        Vector2 ScreenPoint = Camera.main.WorldToScreenPoint(point);
        return ScreenPoint.y > Screen.height;
    }
    public static bool BelowScreen(Vector3 point)
    {
        Vector2 ScreenPoint = Camera.main.WorldToScreenPoint(point);
        return ScreenPoint.y < 0;
    }
    public static Vector3 GetGemUiPosition()
    {
        Vector2 GemUIPos = UIManager.GetGemUiPosition();
        Ray ray = Camera.main.ScreenPointToRay(GemUIPos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 250f, 1 << 10))
        {
            return raycastHit.point;
        }
        return Vector3.zero;
    }

    #endregion

 
    private void FixedUpdate()
    {

    }

}
