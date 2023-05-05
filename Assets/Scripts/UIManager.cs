using DG.Tweening;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public TextMeshProUGUI moveCountText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI dayText;

    public Button undoSwapButton;
    public Button removeCellButton;
    public Image removePanelActive;

    public Transform healtBar;
    public Image healtImage;

    public TextMeshProUGUI earnedMoveCountText;

    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public GameObject blackPanel;
    public Button resumeButton;


    private void OnEnable()
    {
        undoSwapButton.onClick.AddListener(UndoSwapButton);
        removeCellButton.onClick.AddListener(RemoveCellButton);
        undoSwapButton.interactable = false;

        GameSignals.OnMoveCount += GetMoveCount;
        Castle.OnCastleHealt += GetHealt;
        GameSignals.OnScore += GetScore;
        GameSignals.OnDay += GetDay;

        CellSignals.OnMatchCount += OnMatchCount;
        GameSignals.OnGameOver += GameOver;
        GameSignals.OnGameState += DefendingState;
    }


    private void OnDisable()
    {
        GameSignals.OnMoveCount -= GetMoveCount;
        Castle.OnCastleHealt -= GetHealt;
        GameSignals.OnScore -= GetScore;
        GameSignals.OnDay -= GetDay;

        CellSignals.OnMatchCount -= OnMatchCount;
        GameSignals.OnGameOver -= GameOver;
        GameSignals.OnGameState -= DefendingState;
    }

    private void Start()
    {
        if (AdmobRewarded.Instance != null)
        {
            //Action newAct = ResumeButton;
            resumeButton.onClick.AddListener(() => AdmobRewarded.Instance.ShowAd(ResumeButton));
        }
    }

    private void DefendingState(GameStates gameState)
    {
        blackPanel.SetActive(gameState == GameStates.defending);
    }

    private void GetScore(int score)
    {
        goldText.text = score.ToString();
    }

    private void GetHealt(int healt)
    {
        for (int i = 0; i < healtBar.childCount; i++)
        {
            Destroy(healtBar.GetChild(i).gameObject);
        }
        for (int i = 0; i < healt; i++)
        {
            Instantiate(healtImage, healtBar);
        }
    }

    void OnMatchCount(int count)
    {
        undoSwapButton.interactable = count <= 0;
    }

    private void GetDay(int index)
    {
        dayText.text = (index).ToString();
    }

    private void GetMoveCount(int count)
    {
        moveCountText.text = count.ToString();
    }

    private void UndoSwapButton()
    {
        GameSignals.OnUpdateMoveCount?.Invoke(+1);
        CellCommandStack.Instance.Undo();
        undoSwapButton.interactable = false;
    }
    public void RemoveCellButton()
    {
        //removePanelActive.gameObject.SetActive(true);
        removePanelActive.gameObject.SetActive(removePanelActive.gameObject.activeSelf != true);
        GameSignals.OnRemoveCell?.Invoke(removePanelActive.gameObject.activeSelf == true);
    }

    public void EarnedMoveText(Transform startPos , int count)
    {
        Vector3 startPosition = Camera.main.WorldToScreenPoint(startPos.position);
        Vector3 targetPosition = new(-425,800,0);
        earnedMoveCountText.gameObject.SetActive(true);
        earnedMoveCountText.rectTransform.position = startPosition;
        earnedMoveCountText.text = "+" + count.ToString() + "MOVE";
        earnedMoveCountText.rectTransform.DOLocalMove(targetPosition, 3f).OnComplete(()=> earnedMoveCountText.gameObject.SetActive(false));
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.isGameStarting = true;
        //GameSignals.OnGameStarting?.Invoke();
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(0);
        AdmobBanner.Instance.DestroyAd();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    void ResumeButton()
    {
        Debug.Log("RESUME MATCHİNG..........!");

        GameManager.Instance.GameState = GameStates.matching;
        //EnemySignals.OnClearCurrentWave?.Invoke();
        GameManager.Instance.UpdateDay();
        for (int i = 0; i < EnemySpawner.Instance.transform.childCount; i++)
        {
            EnemySpawner.Instance.enemies.ReturnObject(EnemySpawner.Instance.transform.GetChild(i).GetComponent<Enemy>());
        }

        Castle.Instance.Healt = 5;
        //Castle.OnCastleHealt.
        

        gameOverPanel.SetActive(false);
        Debug.Log("RESUME");
    }
}
