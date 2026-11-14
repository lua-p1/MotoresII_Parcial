using UnityEngine;
// Popup "Sin energia". Lo pushean ScreenLevelSelect.BTN_PlayLevel() (menu) y ScreenEnd.BTN_Restart() (nivel).
public class ScreenNoStamina : ScreenBase
{
    public void BTN_WatchAd()
    {
        if (AdsManager.Instance == null) return;   // por si la escena no tiene AdsManager
        if (AdsManager.Instance.ShowAd())
            BTN_Back();                            // cierra el popup; el anuncio se muestra encima
    }
}
