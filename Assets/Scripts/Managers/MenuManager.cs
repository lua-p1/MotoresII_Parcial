using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private string levelSceneName = "Level1";
    public void Play()
    {
        if (!StaminaManager.Instance.UseStamina(1))
        {
            ScreenManager.Instance.Push(ScreenNames.NoStamina);
            return;
        }
        SceneManager.LoadScene(levelSceneName);
    }
}
