using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    #region Singletone
    private static LevelManager _default;
    public static LevelManager Default { get => _default; }
    public LevelManager() => _default = this;
    #endregion

    const string PREFS_KEY_LEVEL_ID = "CurrentLevelCount";
    const string PREFS_KEY_LAST_INDEX = "LastLevelIndex";

    public bool editorMode = false;
    public int CurrentLevelCount => PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1;
    public int CurrentLevelIndex;
    public List<Level> Levels = new List<Level>();
    public Action OnLevelStarted;


    public void Awake()
    {
#if !UNITY_EDITOR
        editorMode = false;
#endif
        if (!editorMode) SelectLevel(PlayerPrefs.GetInt("LastLevelIndex"), true);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(PREFS_KEY_LAST_INDEX, CurrentLevelIndex);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LastLevelIndex", CurrentLevelIndex);
    }


    public void StartLevel()
    {
        //SendStart();
        OnLevelStarted?.Invoke();
    }

    public void RestartLevel()
    {
        //SendRestart();
        SelectLevel(CurrentLevelIndex, false);
    }

    public void NextLevel()
    {
        //SendComplete();
        if (!editorMode)
            PlayerPrefs.SetInt(PREFS_KEY_LEVEL_ID, (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID) + 1));
        SelectLevel(CurrentLevelIndex + 1);
    }

    public void ClearListAtIndex(int levelIndex)
    {
        Levels[levelIndex].LevelPrefab = null;
    }

    public void SelectLevel(int levelIndex, bool indexCheck = true)
    {
        if (indexCheck)
            levelIndex = GetCorrectedIndex(levelIndex);

        if (Levels[levelIndex].LevelPrefab == null)
        {
            Debug.Log("<color=red>There is no prefab attached!</color>");
            return;
        }

        var level = Levels[levelIndex];

        if (level.LevelPrefab)
        {
            SelLevelParams(level);
            CurrentLevelIndex = levelIndex;
        }
    }

    public void PrevLevel() =>
        SelectLevel(CurrentLevelIndex - 1);

    private int GetCorrectedIndex(int levelIndex)
    {
        if (editorMode)
            return levelIndex > Levels.Count - 1 || levelIndex <= 0 ? 0 : levelIndex;
        else
        {
            int levelId = PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID);
            if (levelId > Levels.Count - 1)
            {
                if (Levels.Count > 1)
                {
                    while (true)
                    {
                        levelId = UnityEngine.Random.Range(0, Levels.Count);
                        if (levelId != CurrentLevelIndex) return levelId;
                    }
                }
                else return UnityEngine.Random.Range(0, Levels.Count);
            }
            return levelId;
        }
    }

    private void SelLevelParams(Level level)
    {
        if (level.LevelPrefab)
        {
            ClearChilds();
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Instantiate(level.LevelPrefab, transform);
            }
            else PrefabUtility.InstantiatePrefab(level.LevelPrefab, transform);
            foreach (IEditorModeSpawn child in GetComponentsInChildren<IEditorModeSpawn>())
                child.EditorModeSpawn();
#else
            Instantiate(level.LevelPrefab, transform);
#endif
        }

        if (level.SkyboxMaterial)
        {
            RenderSettings.skybox = level.SkyboxMaterial;
        }
    }

    private void ClearChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject destroyObject = transform.GetChild(i).gameObject;
            DestroyImmediate(destroyObject);
        }
    }



    #region Analitics Events

/*    public void SendStart()
    {
        string content = (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1).ToString();
    }

    public void SendRestart()
    {
        string content = (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1).ToString();
    }

    public void SendComplete()
    {
        string content = (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1).ToString();
    }*/

    #endregion
}

[System.Serializable]
public class Level
{
    public GameObject LevelPrefab;
    public Material SkyboxMaterial;
}