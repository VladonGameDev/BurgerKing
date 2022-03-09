using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GameData")]
public class GameData : DataHolder
{
    #region Singleton

    private static GameData _default;
    public static GameData Default => _default;

    #endregion

    [Header("UI")] 
    public bool enableFpsCounter;
    public float shiftModifier, shiftRange;


    public override void Init()
    {
        _default = this;
    }
}
