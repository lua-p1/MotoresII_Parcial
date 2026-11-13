using UnityEngine;
// Popup "Sin energia". Lo pushean MenuManager.Play() (menu) y ScreenEnd.BTN_Restart() (nivel).
// Ofrece ver un anuncio: el premio lo da AdsManager segun si se completo o se salteo.
public class ScreenNoStamina : ScreenBase
{
    public void BTN_WatchAd()
    {
        if (AdsManager.instance == null) return;   // por si la escena no tiene AdsManager
        if (AdsManager.instance.ShowAd())
            BTN_Back();                            // cierra el popup; el anuncio se muestra encima
    }
}
