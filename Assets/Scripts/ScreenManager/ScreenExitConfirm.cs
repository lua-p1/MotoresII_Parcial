using UnityEngine;
// Confirmacion para salir del juego. Se abre desde el boton Salir del menu.
// Cancelar usa BTN_Back.
public class ScreenExitConfirm : ScreenBase
{
    public void BTN_ConfirmExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // en el editor, Quit no hace nada: paro el Play
#endif
    }
}
