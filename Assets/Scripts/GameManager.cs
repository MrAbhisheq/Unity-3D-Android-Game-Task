using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    public MainPlayer mainPlayer;
    public List<BotPlayer> botPlayers;

    [Header("Initial Platform")]
    public List<PlatformBlock> initialPlatforms;

    [Header("UI Elements")]
    public GameObject playerCount;
    public TextMeshProUGUI playerCountText;
    [Space(5)]
    public GameObject dragToMovePanel;
    public TextMeshProUGUI countdown;
    public Button tapToStartBtn;
    [Space(5)]
    public RectTransform victoryBanner;
    public RectTransform gameOverBanner;
    public TextMeshProUGUI placeNumber;
    public Button retryBtn;

    [Space(10)]
    public Button infoBtn;
    public GameObject infoPanel;
    public Button closeInfoBtn;

    private int playerCountValue;

    public bool isExitable = true;

    private void Start()
    {
        playerCount.SetActive(false);
        dragToMovePanel.SetActive(false);

        victoryBanner.gameObject.SetActive(false);
        gameOverBanner.gameObject.SetActive(false);

        countdown.gameObject.SetActive(false);
        countdown.transform.localScale = Vector3.zero;

        retryBtn.gameObject.SetActive(false);
        retryBtn.onClick.AddListener(OnRetryButtonClicked);

        tapToStartBtn.gameObject.SetActive(true);
        tapToStartBtn.onClick.AddListener(StartGame);

        mainPlayer.ToggleMovement(false);
        foreach (BotPlayer player in botPlayers)
        {
            player.ToggleMovement(false);
            player.gameObject.SetActive(false);
        }

        foreach (PlatformBlock platform in initialPlatforms)
        {
            platform.enabled = false;
        }

        infoBtn.onClick.AddListener(OnInfoBtnClicked);
        closeInfoBtn.onClick.AddListener(OnCloseInfoBtnClicked);
        infoPanel.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (isExitable && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ShowHome()
    {
        isExitable = true;

        playerCount.SetActive(false);
        tapToStartBtn.gameObject.SetActive(true);
        mainPlayer.gameObject.SetActive(true);

        infoBtn.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        isExitable = false;
        StartCoroutine(GameStartSequence());
    }

    IEnumerator GameStartSequence()
    {
        infoBtn.gameObject.SetActive(false);

        tapToStartBtn.gameObject.SetActive(false);
        dragToMovePanel.SetActive(true);
        playerCount.SetActive(true);
        SetPlayerCount(botPlayers.Count + 1);

        mainPlayer.gameObject.SetActive(true);
        foreach (BotPlayer player in botPlayers)
        {
            player.gameObject.SetActive(true);
        }

        countdown.gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            countdown.text = (3 - i).ToString();
            countdown.transform.localScale = Vector3.zero;
            countdown.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
        }

        countdown.text = "GO!";
        countdown.transform.localScale = Vector3.zero;
        countdown.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);

        countdown.gameObject.SetActive(false);
        dragToMovePanel.SetActive(false);

        StartCoroutine(CheckGameState());

        mainPlayer.ToggleMovement(true);
        //mainPlayer.OnPlayerFallen += OnMainPlayerFallen;
        foreach (BotPlayer player in botPlayers)
        {
            player.ToggleMovement(true);
            player.OnPlayerFallen += OnBotPlayerFallen;
        }

        foreach (PlatformBlock platform in initialPlatforms)
        {
            platform.enabled = true;
            StartCoroutine(platform.HideInitialPlatform());
        }
    }

    IEnumerator CheckGameState()
    {
        while (true)
        {
            if (playerCountValue <= 1)
            {
                ShowVictoryPanel();
                mainPlayer.gameObject.SetActive(false);

                HideOtherPlayers();
                yield break;
            }
            else if (mainPlayer.isFallen)
            {
                ShowTryAgainPanel();

                HideOtherPlayers();
                yield break;
            }
            yield return null;
        }

        void HideOtherPlayers()
        {
            foreach (BotPlayer player in botPlayers)
            {
                player.gameObject.SetActive(false);
                player.OnPlayerFallen -= OnBotPlayerFallen;
            }
        }
    }

    void SetPlayerCount(int count)
    {
        Debug.Log("Setting player count to: " + count);

        playerCountValue = count;
        playerCountText.text = playerCountValue.ToString();
    }

    //void OnMainPlayerFallen()
    //{
    //    mainPlayer.OnPlayerFallen -= OnMainPlayerFallen;
    //    ShowTryAgainPanel();
    //}

    void OnBotPlayerFallen()
    {
        SetPlayerCount(--playerCountValue);
    }
    
    public void ShowTryAgainPanel()
    {
        retryBtn.gameObject.SetActive(false);

        placeNumber.text = $"{playerCountValue} place";
        gameOverBanner.gameObject.SetActive(true);

        gameOverBanner.anchoredPosition = new Vector2(0, 100);
        gameOverBanner.DOAnchorPos(new Vector2(0, -250), 1).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            retryBtn.gameObject.SetActive(true);
        });
    }

    public void ShowVictoryPanel()
    {
        retryBtn.gameObject.SetActive(false);

        victoryBanner.gameObject.SetActive(true);

        victoryBanner.anchoredPosition = new Vector2(0, 175);
        victoryBanner.DOAnchorPos(new Vector2(0, -250), 1).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            retryBtn.gameObject.SetActive(true);
        });
    }

    void OnRetryButtonClicked()
    {
        retryBtn.gameObject.SetActive(false);
        ResetGame();
        ShowHome();
        //RestartGame();
    }

    void RestartGame()
    {
        ResetGame();
        StartGame();
    }

    void ResetGame()
    {
        mainPlayer.ResetPlayer();
        foreach (BotPlayer player in botPlayers)
        {
            player.ResetPlayer();
            player.gameObject.SetActive(false);
            player.OnPlayerFallen -= OnBotPlayerFallen;
        }
        victoryBanner.gameObject.SetActive(false);
        gameOverBanner.gameObject.SetActive(false);

        foreach (PlatformBlock platform in initialPlatforms)
        {
            platform.ResetPlatform();
            platform.enabled = false;
        }
    }

    void OnInfoBtnClicked()
    {
        infoPanel.SetActive(true);
    }

    void OnCloseInfoBtnClicked()
    {
        infoPanel.SetActive(false);
    }

}
