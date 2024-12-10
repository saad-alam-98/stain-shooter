using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public ResultComponent ContainerResult;
    public LoadingComponent LoadingComponent;
    public GameObject ContainerGameplay;

    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextTimer;

    public ClothsSpawner[] ClothsSpawners;

    public Image ImageOverlay;

    public Transform SurfExcelLogo;
    public Transform BubbleGun;

    public int Duration = 30;
    public static int Score = 0;

    public static bool isGameRunning;
    public static bool isGameComplete;

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartGame();
    }

    private void StartGame()
    {
        LoadingComponent.gameObject.SetActive(true);
        LoadingComponent.PlaySequence(() =>
        {
            ContainerGameplay.SetActive(true);
            LoadingComponent.gameObject.SetActive(false);
            TextScore.text = "0";
            TextTimer.text = Duration.ToString();
            foreach (var spawner in ClothsSpawners)
            {
                spawner.StartGame();
            }
            PlayStartSequence();
        });
    }

    Coroutine routine;

    public void PlayStartSequence()
    {
        Sequence sequence = DOTween.Sequence();
        bool isNextAnimationStarted = false;

        Tween surfExcelTween = SurfExcelLogo.DOLocalMoveY(-2.56f, 1);

        sequence.Append(surfExcelTween.OnUpdate(() =>
        {
            float progress = surfExcelTween.ElapsedPercentage();
            if (progress >= 0.5f && !isNextAnimationStarted)
            {
                isNextAnimationStarted = true;

                BubbleGun.DOLocalMoveY(-3.2f, 1);
            }
        }));

        sequence.AppendInterval(0.5f);

        sequence.OnComplete(() =>
        {
            routine = StartCoroutine(GameTimer());
            isGameRunning = true;
        });

        sequence.Play();
    }

    private IEnumerator GameTimer()
    {
        TextTimer.text = Duration.ToString();

        while(Duration > 0)
        {
            yield return new WaitForSeconds(1);
            Duration -= 1;
            TextTimer.text = Duration.ToString();
        }
        EndGame(30);
    }

    private void EndGame(int Duration)
    {
        StopCoroutine(routine);
        routine = null;
        isGameComplete = true;
        isGameRunning = false;
        SoundManager.instance.OnGameWon();
        ContainerResult.Setup(Duration, Score);
    }

    public void AddScore()
    {
        Score += 1;
        TextScore.text = Score.ToString();

        if (Score >= 10)
        {
            EndGame(30 - Duration);

        }
    }

    public void ClearAndRestart()
    {
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        ImageOverlay.DOFade(1, 1);
        yield return new WaitForSeconds(1);

        foreach (var spawner in ClothsSpawners)
        {
            spawner.EndGame();
        }
        ContainerResult.gameObject.SetActive(false);
        Dispose();
        StartGame();
        isGameComplete = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Dispose()
    {
        Score = 0;
        Duration = 30;
        instance = null;
    }
}
