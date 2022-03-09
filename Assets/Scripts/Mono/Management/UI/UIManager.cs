using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Active { get; private set; }
    public UIManager()
    {
        Active = this;
    }

    private const string LEVEL_NAME = "LEVEL";
    private const string LEVEL_FAILED = "FAILED";
    private const string LEVEL_DONE = "DONE";
    private const string GEM_COLLECTED = "GemCollected";
    private const string ANIM_ID = "State";
    private const string ANIM_COMBO = "Combo";
    private const string ANIM_TUTOR = "TutorNum";
    private const string ANIM_TUTORTEXT = "TutorTextNum";
    private const string ANIM_CURSORSHOW = "ShowCursor";
    private const string ANIM_CURSORCLICK = "ClickCursor";
    private int GemCollected;
    private int GemCollectedInRow;
    public bool isGameStarted = false;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI StartLevelNum;
    [SerializeField] private TextMeshProUGUI DoneLevelNum;
    [SerializeField] private TextMeshProUGUI FailedLevelNum;
    [SerializeField] private TextMeshProUGUI GemNum;
    [SerializeField] private TextMeshProUGUI PlusNum;
    [Header("Components")]
    [SerializeField] private RectTransform GemIcon;
    public static Vector2 GetGemUiPosition()
    {
        return Active.GemIcon.position;
    }
    [SerializeField] private RectTransform PlusIcon;
    [SerializeField] private RectTransform Cursor;
    [SerializeField] private RectTransform CursorHand;
    [SerializeField] private RectTransform Title;
    public CanvasScaler canvasScaler;
    private Animator _animator;

    private Coroutine ShowComboCoroutine;
    public enum UIState {InGame, Start, Failed, Done};
    [SerializeField] private UIState CurrantState = UIState.Start;
    private void SetState(UIState state)
    {
        CurrantState = state;

        _animator.SetInteger(ANIM_ID, (int)CurrantState);
    }

    public void Play()
    {
        if (CurrantState != UIState.Start)
            return;
        LevelManagement.Default.StartGame();
        isGameStarted = true;

        SoundManagment.PlaySound("click");
    }
    public void Restart()
    {
        if (CurrantState != UIState.Failed)
            return;
        LevelManagement.Default.RestartLevel();

        SetState(UIState.Start);
        SoundManagment.PlaySound("click");
    }
    public void GameRestart()
    {
        if (CurrantState != UIState.InGame)
            return;
        LevelManagement.Default.RestartLevel();
        TurnTutor(-1);
        SetState(UIState.Start);
        SoundManagment.PlaySound("click");
    }
    public void Next()
    {
        if (CurrantState != UIState.Done)
            return;
        LevelManagement.Default.NextLevel();
        SetState(UIState.Start);
        SoundManagment.PlaySound("click");
        isGameStarted = false;
    }

    public void TurnTutor(int num)
    {
        _animator.SetInteger(ANIM_TUTOR, num);
    }
    public void TurnTutorText(int num)
    {
        _animator.SetInteger(ANIM_TUTORTEXT, num);
    }


    private void OnGameStarted()
    {
        SetState(UIState.InGame);
    }
    private void OnGameFailed()
    {
        SetState(UIState.Failed);
    }
    public void OnGameDone()
    {
        SetState(UIState.Done);
    }

    /*public void OnGemCollected(ICollected gem)
    {
        Vector2 Pos = Camera.main.WorldToScreenPoint(gem.GemTransform.position);
        GemCollected++;
        SetGemNum();
        StartCoroutine(GemFly(Pos));
        if(ShowComboCoroutine != null)
        {
            StopCoroutine(ShowComboCoroutine);
        }
        ShowComboCoroutine = StartCoroutine(ShowComboCour(Pos));
    }*/
    private void SetGemNum()
    {
        GemNum.text = GemCollected.ToString();
    }
    private IEnumerator GemFly(Vector3 StartPos)
    {
        GameObject gem = Instantiate(DataHolder.Data.GemIcon, StartPos, Quaternion.identity, transform);

        gem.transform.SetAsFirstSibling();
        yield return new WaitForSeconds(DataHolder.Data.GemDelayToFly);
        while(((Vector2)gem.transform.position - (Vector2)GemIcon.transform.position).magnitude > 1f)
        {
            float Speed = 1 / Mathf.Sqrt(((Vector2)gem.transform.position - (Vector2)GemIcon.transform.position).magnitude);
            gem.transform.position = Vector2.Lerp(gem.transform.position, GemIcon.transform.position, DataHolder.Data.GemFlySpeed * (0.1f + Speed));
            yield return new WaitForFixedUpdate();
        }
        _animator.Play(GEM_COLLECTED, 1, 0);
        Destroy(gem);
        yield break;
    }
    private IEnumerator ShowComboCour(Vector3 StartPos)
    {
        GemCollectedInRow++;
        if(StartPos.x >= Screen.width * 0.5f)
        {
            StartPos += Vector3.right * 100;
        }
        else if(StartPos.x < Screen.width * 0.5f)
        {
            StartPos += Vector3.right * -100;
        }
        PlusIcon.transform.position = StartPos;
        _animator.Play(ANIM_COMBO, 2, 0);
        PlusNum.text = "+" + GemCollectedInRow.ToString();
        yield return new WaitForSeconds(1f);
        GemCollectedInRow = 0;
        yield break;
    }

    private void SubscribeForAction()
    {
        GameManagement.OnGameFailed += OnGameFailed;
        GameManagement.OnGameWin += OnGameDone;
        GameManagement.OnGameStarted += OnGameStarted;
    }
    private void UnsubscribeForAction()
    {
        GameManagement.OnGameFailed -= OnGameFailed;
        GameManagement.OnGameWin -= OnGameDone;
        GameManagement.OnGameStarted -= OnGameStarted;
    }



    public void SetLevelText() //Calls from NULL_State in animations
    {
        StartLevelNum.text = LEVEL_NAME + " " + (LevelManagement.Default.CurrentLevelIndex + 1);
        DoneLevelNum.text = LEVEL_NAME + " " + (LevelManagement.Default.CurrentLevelIndex + 1) + " " + LEVEL_DONE;
        FailedLevelNum.text = LEVEL_NAME + " " + (LevelManagement.Default.CurrentLevelIndex + 1) + " " + LEVEL_FAILED;
    }

    private void Init()
    {
        Active = this;
        _animator = GetComponent<Animator>();

        SetLevelText();
        SetGemNum();
        GemCollectedInRow = 0;

        TurnTutor(-1);
        Title.gameObject.SetActive(DataHolder.Data.ShowTitle);
    }

    private void FixedUpdate()
    {

    }
    private void Awake() => Init();
 

    private void Start()
    {
        SubscribeForAction();
    }
    private void OnDestroy()
    {
        UnsubscribeForAction();
    }
}
