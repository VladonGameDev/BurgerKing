using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float ShakeTime;
    [SerializeField] private float MaxShakeAngle;
    [SerializeField] private AnimationCurve ShakeCurve;
    [Header("Components")]
    [SerializeField] private Transform Main;

    public void DoShake()
    {
        if (ShakeCoroutine == null)
        {
            ShakeCoroutine = StartCoroutine(ShakeCour(1));
        }
    }
    public void DoShake(float Power)
    {
        if (ShakeCoroutine == null)
        {
            ShakeCoroutine = StartCoroutine(ShakeCour(Power));
        }
    }

    private Coroutine ShakeCoroutine;
    private IEnumerator ShakeCour(float Power)
    {
        float shakeTime = ShakeTime * Power;
        float currantTime = 0;

        float Angle = 0;

        Quaternion prevRotation = Main.rotation;

        while (currantTime < shakeTime)
        {
            Angle = ShakeCurve.Evaluate(currantTime / shakeTime) * MaxShakeAngle * Power * Mathf.Deg2Rad;

            Main.rotation = new Quaternion(Main.rotation.x, Main.rotation.y, prevRotation.z + Angle, Main.rotation.w);


            currantTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Main.rotation = prevRotation;

        ShakeCoroutine = null;
        yield break;
    }

    private void Start()
    {
        if(Main == null)
        {
            Main = transform;
        }
    }
}
