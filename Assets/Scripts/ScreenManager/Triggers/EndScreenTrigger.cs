using UnityEngine;
// Abre la pantalla final que corresponde cuando termina el nivel.
// GameOver lo dispara PlayerHealth.Die() y LevelComplete lo dispara LevelManager.CompleteLevel().
public class EndScreenTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.GameOver, ShowGameOver);
        EventManager.SubscribeToEvent(EventType.LevelComplete, ShowVictory);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.GameOver, ShowGameOver);
        EventManager.UnsubscribeToEvent(EventType.LevelComplete, ShowVictory);
    }
    private void ShowGameOver(params object[] parameters)
    {
        Show("GameOverScreen");
    }
    private void ShowVictory(params object[] parameters)
    {
        Show("VictoryScreen");
    }
    private void Show(string screenName)
    {
        if (ScreenManager.Instance.StackContainsType<ScreenEnd>()) return;
        ScreenManager.Instance.Push(screenName);
    }
}
