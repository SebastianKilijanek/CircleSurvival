using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.States;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameState gameState;
    public static GameState GameState { get => gameState; private set => gameState = value; }
    private static GameEventBus gameEventBus;
    public static GameEventBus GameEventBus { get => gameEventBus; private set => gameEventBus = value; }

    void Awake()
    {
        GameState = GameStatePersistance.LoadGameStateData();
        GameEventBus = new GameEventBus();

        GameEventBus.On<GameStartedEvent>(OnGameStartedEvent);
        GameEventBus.On<GameEndedEvent>(OnGameEndedEvent);
        GameEventBus.On<NewHighScoreEvent>(OnNewHighScoreEvent);
    }

    private void OnGameStartedEvent(GameStartedEvent arg)
    {
        SceneManager.LoadSceneAsync("GameplayScene", LoadSceneMode.Additive);
    }

    private void OnGameEndedEvent(GameEndedEvent arg)
    {
        SceneManager.UnloadSceneAsync("GameplayScene");
    }

    private void OnNewHighScoreEvent(NewHighScoreEvent arg)
    {
        GameStatePersistance.SaveGameStateData(gameState);
    }

    void OnDestroy()
    {
        GameEventBus.Off<GameStartedEvent>(OnGameStartedEvent);
        GameEventBus.Off<GameEndedEvent>(OnGameEndedEvent);
        GameEventBus.Off<NewHighScoreEvent>(OnNewHighScoreEvent);
    }
}
