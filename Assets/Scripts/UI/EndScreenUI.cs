using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private EventType showOnEvent;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(showOnEvent, Show);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(showOnEvent, Show);
    }

    private void Show(params object[] parameters)
    {
        panel.SetActive(true);
    }

    public void Restart()
    {
        Debug.Log("Reinciar");
    }
}
