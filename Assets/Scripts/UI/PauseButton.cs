using UnityEngine;
public class PauseButton : MonoBehaviour
{
    public void BTN_Pause()
    {
        if (!GameManager.IsPlaying) return;
        if (ScreenManager.Instance.StackContainsType<ScreenPause>()) return;   // ya esta abierta
        ScreenManager.Instance.Push(ScreenNames.Pause);
    }
}


