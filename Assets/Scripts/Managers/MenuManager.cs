using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private string levelSceneName = "Level1";
    public void Play()
    {
        if (!StaminaManager.instance.UseStamina(1))
        {
            ScreenManager.Instance.Push("NoStaminaScreen");
            return;
        }
        SceneManager.LoadScene(levelSceneName);
    }
}
