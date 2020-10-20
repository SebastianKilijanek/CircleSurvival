using System;
using System.Collections;
using Assets.Scripts.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private HUDView hUDView;
    private Coroutine timerCoroutine;

    private void Awake()
    {
        hUDView.BlackScreenGroup.alpha = 0;
        hUDView.NewHighScoreTxtRT.gameObject.SetActive(false);
        hUDView.BackButton.onClick.RemoveAllListeners();
        hUDView.BackButton.onClick.AddListener(OnBackButtonClicked);
        hUDView.BackButton.gameObject.SetActive(false);

        GameManager.GameEventBus.On<GameOverEvent>(OnGameOverEvent);
        GameManager.GameEventBus.On<NewHighScoreEvent>(OnNewHighScoreEvent);
    }

    private void Start()
    {
        timerCoroutine = StartCoroutine(SetTimer());
    }

    IEnumerator SetTimer()
    {
        while (true)
        {
            float minutes = Mathf.FloorToInt(GameManager.GameState.TimeScore / 60);
            float seconds = Mathf.FloorToInt(GameManager.GameState.TimeScore % 60);
            hUDView.TimeScoreTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
    }

    private void OnBackButtonClicked()
    {
        GameManager.GameEventBus.Trigger<GameEndedEvent>(new GameEndedEvent());
    }

    private void OnGameOverEvent(GameOverEvent arg)
    {
        StopCoroutine(timerCoroutine);

        hUDView.TimeSurvivedTxt.text = hUDView.TimeScoreTxt.text;
        hUDView.BackButton.gameObject.SetActive(true);

        StartCoroutine(UITweener.FadeCanvasGroup(hUDView.BlackScreenGroup, 0f, 1f, 1f));
    }

    private void OnNewHighScoreEvent(NewHighScoreEvent arg)
    {
        hUDView.NewHighScoreTxtRT.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        GameManager.GameEventBus.Off<GameOverEvent>(OnGameOverEvent);
        GameManager.GameEventBus.Off<NewHighScoreEvent>(OnNewHighScoreEvent);
    }
}

[Serializable]
public class HUDView
{
    public TextMeshProUGUI TimeScoreTxt;
    public CanvasGroup BlackScreenGroup;
    public TextMeshProUGUI TimeSurvivedTxt;
    public RectTransform NewHighScoreTxtRT;
    public Button BackButton;
}