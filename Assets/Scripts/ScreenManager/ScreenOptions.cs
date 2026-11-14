using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
// Opciones: volumen general del juego (Master del AudioMixer). La logica del volumen esta en VolumeSettings.
// Se abre desde el menu y desde la pausa con BTN_Screen("OptionsScreen"); Volver usa BTN_Back.
public class ScreenOptions : ScreenBase
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;   // Min 0.0001, Max 1
    private void Start()
    {
        volumeSlider.value = VolumeSettings.Load();   // muestra el volumen guardado
        volumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    public void SetMasterVolume(float value)
    {
        VolumeSettings.Apply(audioMixer, value);
        VolumeSettings.Save(value);
    }
    public override void Release()
    {
        PlayerPrefs.Save();   // guarda en disco una vez, al cerrar Opciones
        base.Release();
    }
}
