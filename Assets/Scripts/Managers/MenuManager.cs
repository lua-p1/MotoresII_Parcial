using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject noStaminaPanel;
    [SerializeField] private string levelSceneName = "Level1";
    public void Play()
    {
        if (!StaminaManager.instance.UseStamine(1))
        {
            noStaminaPanel.SetActive(true);
            return;
        }
        SceneManager.LoadScene(levelSceneName);
    }
    public void CloseNoStaminaPanel()
    {
        noStaminaPanel.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
