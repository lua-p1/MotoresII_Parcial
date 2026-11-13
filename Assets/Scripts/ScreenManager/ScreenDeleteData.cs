using UnityEngine;
using UnityEngine.SceneManagement;
// Confirmacion para borrar los datos guardados. Se abre desde Opciones.
// Cancelar usa BTN_Back (vuelve a Opciones).
public class ScreenDeleteData : ScreenBase
{
    [SerializeField] private string menuSceneName = "MainMenu";
    public void BTN_ConfirmDelete()
    {
        // El volumen es una opcion del jugador, no progreso: lo guardo antes de borrar
        float volume = PlayerPrefs.GetFloat(ScreenOptions.VolumeKey, 1f);
        PlayerPrefs.DeleteAll();                                   // monedas, energia, corazon pendiente
        PlayerPrefs.SetFloat(ScreenOptions.VolumeKey, volume);
        PlayerPrefs.Save();
        SceneManager.LoadScene(menuSceneName);                     // los managers vuelven a cargar desde cero
    }
}
