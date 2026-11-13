using UnityEngine;
using UnityEngine.SceneManagement;
// Confirmacion para abandonar el nivel y volver al menu. Se abre desde la pausa.
// Cancelar usa BTN_Back (vuelve a la pausa).
public class ScreenMenuConfirm : ScreenBase
{
    [SerializeField] private string menuSceneName = "MainMenu";
    public void BTN_ConfirmMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
