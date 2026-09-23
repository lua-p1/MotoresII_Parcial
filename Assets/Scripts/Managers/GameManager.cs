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
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.GameOver, GameOver);
    }
    void Start()
    {
        CurrentState = GameState.MainMenu;
    }
    public void StartGame()
    {
        if (CurrentState == GameState.Playing) return;
        CurrentState = GameState.Playing;
        EventManager.TriggerEvent(EventType.GameStart);
        Debug.Log("Game Started");
    }
    private void GameOver(params object[] parameters)
    {
        CurrentState = GameState.GameOver;
        Debug.Log("Game Over");
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.GameOver, GameOver);
    }
}