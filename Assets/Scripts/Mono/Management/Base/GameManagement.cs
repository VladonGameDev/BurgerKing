using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Active { get; private set; }
    public GameManagement()
    {
        Active = this;
    }

    public enum StateType { NotStarted, Started, Failed, Done }
    public static StateType GameState { get; set; }

    public static Transform LevelTransform => LevelManagement.Default.transform.GetChild(0);


    //private List<ICollected> Gems = new List<ICollected>();
    public int GemsCount { get; private set; }
    public int GemsCollected { get; private set; }
    public int HostageCount { get; private set; }
    public int HostageCollected { get; private set; }
    public void IncreaseHostage()
    {
        HostageCollected++;
    }
    public void DecreaseHostage()
    {
        HostageCollected--;
        if(HostageCollected < 0)
        {
            HostageCollected = 0;
        }
    }

    public static float GetHightLimitY()
    {
        Vector2 ScreenPoint = new Vector2(0, Screen.height * DataHolder.Data.HightLimitY);
        Ray ray = Camera.main.ScreenPointToRay(ScreenPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 250f, 1 << 10))
        {
            return raycastHit.point.y;
        }
        return 0;
    }
    public static float GetLowLimitY()
    {
        Vector2 ScreenPoint = new Vector2(0, Screen.height * (1 + DataHolder.Data.LowLimitY));
        Ray ray = Camera.main.ScreenPointToRay(ScreenPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 250f, 1 << 10))
        {
            return raycastHit.point.y;
        }
        return 0;
    }

    public delegate void GameAction();
    public static GameAction OnGameStarted;
    public static GameAction OnGameWin;
    public static GameAction OnGameFailed;

    public static float RandomOne()
    {
        return Random.Range(0, 2) == 0 ? 1 : -1 * (Random.Range(0.5f, 1f));
    }
    public static Vector3 RandomVectorXY()
    {
        return new Vector3(RandomOne(), RandomOne(), 0).normalized;
    }

    public void StartGame()
    {
        OnGameStarted?.Invoke();
        GameState = StateType.Started;
    }
    public void Init()
    {
        GameState = StateType.NotStarted;

    }

    /*private void GetGemsOnScene()
    {
        Gems.Clear();
        GameObject[] gem = GameObject.FindGameObjectsWithTag("Gem");
        foreach(GameObject G in gem)
        {
            ICollected temp = G.GetComponent<ICollected>();
            if (temp != null)
            {
                Gems.Add(temp);
                temp.Init();

                temp.SubscribeFor(OnGemCollected);
            }
        }
        GemsCount = Gems.Count;
        GemsCollected = 0;
    }*/

    public void OnGameWinAction()
    {
        if (GameState == StateType.Done)
            return;
        GameState = StateType.Done;
        StartCoroutine(OnGameWinCour());
    }
    private IEnumerator OnGameWinCour()
    {
        OnGameWin?.Invoke();

        SoundManagment.PlaySound("win");
        //StartCoroutine(ConfittyCour());

        
        yield break;
    }
    private IEnumerator ConfittyCour()
    {
        for(int i = 0; i < DataHolder.Data.ConfittyNum && GameState == StateType.Done; i++)
        {
            Vector3 Position = Vector3.zero; //_danceZone.position + new Vector3(RandomOne(), 0, RandomOne()) * MainData.ConfittyRadius + Vector3.up * 3;
            Instantiate(DataHolder.Data.GetRandomConfitty(), Position, Quaternion.identity, LevelTransform);
            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }

    public void OnGameFailedAction()
    {
        if (GameState == StateType.Failed)
            return;
        GameState = StateType.Failed;
        OnGameFailed?.Invoke();
    }

    /*private void OnGemCollected(ICollected gem)
    {
        GemsCollected++;
        UIManager.Active.OnGemCollected(gem);
    }*/

}
