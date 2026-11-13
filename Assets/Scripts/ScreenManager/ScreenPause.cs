using UnityEngine;
using UnityEngine.SceneManagement;
// Menu de pausa. Mientras esta abierto, ScreenGameplay tiene pausado todo lo IPauseable de Root.
// Reanudar usa BTN_Back (heredado de ScreenBase).
public class ScreenPause : ScreenBase
{
    [SerializeField] private string menuSceneName = "MainMenu";
    public void BTN_Menu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
