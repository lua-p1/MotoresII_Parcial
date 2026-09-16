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
}