using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenMenuConfirm : ScreenBase
{
    [SerializeField] private string menuSceneName = "MainMenu";
    public void BTN_ConfirmMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
