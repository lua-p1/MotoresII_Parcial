using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { MainMenu, Playing, GameOver }
    public GameState CurrentState { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        CurrentState = GameState.MainMenu;
    }
    public void StartGame()
    {
        if (CurrentState == GameState.Playing) return;
        CurrentState = GameState.Playing;
        EventManager.OnGameStart?.Invoke();
        Debug.Log("Game Started");
    }
    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        EventManager.OnGameOver?.Invoke();
        Debug.Log("Game Over");
    }
}