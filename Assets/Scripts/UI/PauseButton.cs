using UnityEngine;
public class PauseButton : MonoBehaviour
{
    public void BTN_Pause()
    {
        var state = GameManager.Instance.CurrentState;
        if (state == GameManager.GameState.GameOver || state == GameManager.GameState.Victory) return;
        if (ScreenManager.Instance.StackContainsType<ScreenPause>()) return;   // ya esta abierta
        ScreenManager.Instance.Push("PauseScreen");
    }
}


