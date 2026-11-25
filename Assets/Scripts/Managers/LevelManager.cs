using UnityEngine;
//Controla el objetivo del nivel: cuenta los enemigos derrotados y, al llegar al objetivo, avisa que se completo.
//Se comunica solo por eventos (escucha GameStart y EnemyKilled, dispara EnemyCountChanged y LevelComplete).
public class LevelManager : MonoBehaviour
{
    [Header("Objetivo del Nivel")]
    [Tooltip("Cantidad de enemigos que el jugador debe derrotar para ganar")]
    [SerializeField] private int targetEnemiesToKill = 60;
    private int _currentEnemiesKilled = 0;
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.GameStart, HandleGameStart);
        EventManager.SubscribeToEvent(EventType.EnemyKilled, HandleEnemyKilled);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.GameStart, HandleGameStart);
        EventManager.UnsubscribeToEvent(EventType.EnemyKilled, HandleEnemyKilled);
    }
    private void HandleGameStart(params object[] parameters)
    {
        _currentEnemiesKilled = 0;
        EventManager.TriggerEvent(EventType.EnemyCountChanged, _currentEnemiesKilled, targetEnemiesToKill);
    }
    private void HandleEnemyKilled(params object[] parameters)
    {
        // Ignorar si no estamos en estado de juego
        if (!GameManager.IsPlaying)
            return;
        _currentEnemiesKilled++;
        // Comprueba si se alcanzo el objetivo
        if (_currentEnemiesKilled >= targetEnemiesToKill)
        {
            CompleteLevel();
        }
        EventManager.TriggerEvent(EventType.EnemyCountChanged, _currentEnemiesKilled, targetEnemiesToKill);
    }
    private void CompleteLevel()
    {
        EventManager.TriggerEvent(EventType.LevelComplete);
    }
}