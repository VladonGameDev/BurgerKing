using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data")]
public class GameData : ScriptableObject
{
    [Header("Dunamic Settings")]
    public bool MuteMusic;
    public bool MuteEffect;
    public bool ShowTitle;
    [Header("Hard Bounce Settings")]
    public bool LostAllHostages;
    [Tooltip("Это максимальный компонент y в векторе (x, y)")]public float MaxDirY;
    public float ManOffsetToBounce;
    [Range(1, 10f)]
    public float ManTrapImulsePower;
    [Range(1, 10f)]
    public float HostageTrapImulsePower;
    [Range(0, 1f)]
    public float Bounce;
    [Range(0, 0.1f)]
    public float Gravity;
    [Range(0, 0.1f)]
    public float AirDrag;
    public int MaxBounce;
    [Header("Rope Hands")]
    public float DelaySimulationTime;
    [Range(0, 1f)]
    public float BounceTime;
    [Range(0, 25f)]
    public float BounceSpeed;
    [Range(0, 1f)]
    public float HandLenght;
    public Vector3 HandGravity;
    [Range(0, 1f)] public float SmoothX;
    [Range(0, 1f)] public float SmoothY;
    [Range(0, 1f)] public float SmoothZ;
    [Header("Limits")]
    [Range(0, 1f)] public float HightLimitY;
    [Range(-1f, 0)] public float LowLimitY;
    [HideInInspector] public float MoveLimitX;
    [Range(0, 1f)] public float MoveLimitXOffset;
    public float MoveLimitY;
    [HideInInspector]public float MoveLimitYOffset;
    public float CenterLimitX;
    public float FallTime;
    [Header("Hostage Settings")]
    [Range(0, 1f)]
    public float HostageLetOutFlySpeed;
    [Range(0, 1f)]
    public float HostageMinDistance;
    public int MoneyForHostage;
    [Range(1, 25f)] public float SpeedLimit;
    public float HostageHeight;
    public float HostageMaxOffset;
    [Header("Camera Settings")]
    public float ShakeTime;
    public float MaxShakeAngle;
    public AnimationCurve ShakeCurve;
    [Header("Finish Animation")]
    public int ConfittyNum;
    public float ConfittyRadius;
    [Range(0f, 1f)]
    public float CameraFinalSpeed;
    public AnimationCurve BounceCurve;
    public AnimationCurve FlyCurve;
    public AnimationCurve MoveCurve;
    [SerializeField] private Material[] WinTowerPlateMaterial;
    public Material GetWinPlateMaterial(int index)
    {
        if (index < 0)
        {
            index = 0;
        }
        else if(index > WinTowerPlateMaterial.Length - 1)
        {
            index = WinTowerPlateMaterial.Length - 1;
        }
        return WinTowerPlateMaterial[index];
    }
    public Material WinTowerBorder;
    [Header("Gem Settings")]
    public float GemFlySpeed;
    public float GemDelayToFly;
    [Header("Prefabs")]
    public GameObject GemIcon;
    [Header("Particle")]
    public ParticleSystem GemCollected;
    public ParticleSystem HitParticle;
    public ParticleSystem[] ConfittyParticle;
    public ParticleSystem GetRandomConfitty()
    {
        return ConfittyParticle[Random.Range(0, ConfittyParticle.Length)];
    }
}
