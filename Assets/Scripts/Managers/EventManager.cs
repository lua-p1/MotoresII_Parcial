using System;

public static class EventManager
{
    #region Input
    public static Action<float> OnSwipeHorizontal;
    public static Action OnSwipeUp;
    public static Action OnSwipeDown;
    #endregion

    #region Game States
    public static Action OnGameStart;
    public static Action OnGameOver;
    public static Action OnLevelComplete;
    #endregion

    #region Enemy & Level Events
    public static Action OnEnemyKilled;

    /// <summary>
    /// Evento para la UI: devuelve (enemigosDerrotadosActuales, enemigosTotalesRequeridos)
    /// </summary>
    public static Action<int, int> OnEnemyCountChanged;
    #endregion
}