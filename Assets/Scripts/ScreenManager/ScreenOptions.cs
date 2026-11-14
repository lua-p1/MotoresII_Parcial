using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
// Opciones: volumen general del juego (volumen general (Master del AudioMixer), guardado en PlayerPrefs).
// Se abre desde el menu y desde la pausa con BTN_Screen("OptionsScreen"); Volver usa BTN_Back.
public class ScreenOptions : ScreenBase
{
    public const string VolumeKey = SaveKeys.MasterVolume;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;   // Min 0.0001, Max 1
    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(VolumeKey, 1f);   // muestra el volumen guardado
        volumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    public void SetMasterVolume(float value)
    {
        ApplyVolume(audioMixer, value);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
    public static void ApplyVolume(AudioMixer mixer, float value)
    {
        mixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
    public override void Release()
    {
        PlayerPrefs.Save();   // guarda en disco una vez, al cerrar Opciones
        base.Release();
    }
}
