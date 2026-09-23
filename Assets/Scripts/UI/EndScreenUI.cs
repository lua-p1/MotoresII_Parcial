using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private EventType showOnEvent;
    [SerializeField] private GameObject panel;
    [SerializeField] private string menuSceneName = "MainMenu";
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
