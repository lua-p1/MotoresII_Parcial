using UnityEngine;
using UnityEngine.SceneManagement;
// Pantalla final (Game Over / Victoria). Se carga desde Resources con el ScreenManager.
public class ScreenEnd : ScreenBase
{
    [SerializeField] private string menuSceneName = "MainMenu";
    public void BTN_Restart()
    {
        if (!StaminaManager.instance.UseStamina(1))
        {
            ScreenManager.Instance.Push(ScreenNames.NoStamina);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BTN_Menu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}