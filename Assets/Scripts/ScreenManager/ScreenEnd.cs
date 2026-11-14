using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
// Pantalla final (Game Over / Victoria). Se carga desde Resources con el ScreenManager.
public class ScreenEnd : ScreenBase
{
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private TMP_Text rewardText;   // solo en VictoryScreen; en GameOverScreen queda vacio
    public void BTN_Restart()
    {
        if (!StaminaManager.Instance.UseStamina(1))
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
    public override void Activate()
    {
        base.Activate();
        if (rewardText != null)
            rewardText.text = $"+{RemoteConfigManager.CoinsPerWin} monedas";
    }
}