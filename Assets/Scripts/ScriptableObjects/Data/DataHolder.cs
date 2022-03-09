using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public DataHolder()
    {
        Active = this;
    }
    public static DataHolder Active { get; private set; }
    public static GameData Data { get => Active._data; }
    [SerializeField] private GameData _data;
}
