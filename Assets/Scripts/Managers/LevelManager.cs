using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Objetivo del Nivel")]
    [Tooltip("Cantidad de enemigos que el jugador debe derrotar para ganar")]
    [SerializeField] private int targetEnemiesToKill = 5;

    private int _currentEnemiesKilled = 0;

    private void Awake()
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
        EventManager.OnGameStart += HandleGameStart;
        EventManager.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= HandleGameStart;
        EventManager.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void Start()
    {
        LogLevelObjective();
    }

    private void HandleGameStart()
    {
        _currentEnemiesKilled = 0;
        LogLevelObjective();
    }

    private void LogLevelObjective()
    {
        Debug.Log($"<color=cyan>[NIVEL INICIADO]</color> Objetivo: Eliminar {targetEnemiesToKill} enemigos. Llevas: {_currentEnemiesKilled}/{targetEnemiesToKill}");
    }

    private void HandleEnemyKilled()
    {
        // Ignorar si no estamos en estado de juego
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        _currentEnemiesKilled++;

        // Informa por consola el progreso de bajas
        Debug.Log($"<color=yellow>[ENEMIGO ELIMINADO]</color> Progreso del nivel: <b>{_currentEnemiesKilled} / {targetEnemiesToKill}</b>");

        // Comprueba si se alcanzó el objetivo
        if (_currentEnemiesKilled >= targetEnemiesToKill)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        Debug.Log($"<color=green><b>¡OBJETIVO COMPLETADO! Has eliminado a los {targetEnemiesToKill} enemigos.</b></color>");

        // Emite el evento global de victoria
        EventManager.OnLevelComplete?.Invoke();
    }
}