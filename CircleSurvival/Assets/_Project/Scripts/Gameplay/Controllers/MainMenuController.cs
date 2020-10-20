using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private MainMenuView mainMenuView;

    void Awake()
    {
        mainMenuView.PlayButton.onClick.RemoveAllListeners();
        mainMenuView.PlayButton.onClick.AddListener(OnPlayButtonClicked);

        GameManager.GameEventBus.On<GameEndedEvent>(OnGameEndedEvent);
    }

    void Start()
    {
        SetBestSoreTxt();
        StartCoroutine(UITweener.FadeImage(mainMenuView.BlackScreen, 1, 0, 1f));
    }

    private void SetBestSoreTxt()
    {
        float minutes = Mathf.FloorToInt(GameManager.GameState.BestTimeScore / 60);
        float seconds = Mathf.FloorToInt(GameManager.GameState.BestTimeScore % 60);
        mainMenuView.BestTimeScoreTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnPlayButtonClicked()
    {
        GameManager.GameEventBus.Trigger<GameStartedEvent>(new GameStartedEvent());
        mainMenuView.RootCanvas.SetActive(false);
    }

    private void OnGameEndedEvent(GameEndedEvent arg)
    {
        mainMenuView.RootCanvas.SetActive(true);
        SetBestSoreTxt();
        StartCoroutine(UITweener.FadeImage(mainMenuView.BlackScreen, 1, 0, 1f));
    }

    private void OnDestroy()
    {
        GameManager.GameEventBus.Off<GameEndedEvent>(OnGameEndedEvent);
    }
}

[Serializable]
public class MainMenuView
{
    public GameObject RootCanvas;
    public Button PlayButton;
    public TextMeshProUGUI BestTimeScoreTxt;
    public Image BlackScreen;
}