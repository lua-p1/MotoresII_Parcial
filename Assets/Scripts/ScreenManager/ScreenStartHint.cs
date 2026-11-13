public class ScreenStartHint : ScreenBase
{
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.GameStart, Close);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.GameStart, Close);
    }
    private void Close(params object[] parameters)
    {
        BTN_Back();
    }
}
