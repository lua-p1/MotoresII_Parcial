using UnityEngine;
public class StartHintUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private void Awake()
    {
        panel.SetActive(true);
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.GameStart, Hide);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.GameStart, Hide);
    }
    private void Hide(params object[] parameters)
    {
        panel.SetActive(false);
    }
}
