using UnityEngine.SceneManagement;
// Selector de niveles. Se abre desde el boton Jugar del menu con BTN_Screen("LevelSelectScreen"); Volver usa BTN_Back.
// Los niveles 2 y 3 son botones con Interactable apagado (se habilitan cuando existan las escenas).
public class ScreenLevelSelect : ScreenBase
{
    public void BTN_PlayLevel(string sceneName)
    {
        if (!StaminaManager.Instance.UseStamina(1))
        {
            ScreenManager.Instance.Push(ScreenNames.NoStamina);
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
