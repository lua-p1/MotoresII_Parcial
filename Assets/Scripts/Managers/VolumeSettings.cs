using UnityEngine;
using UnityEngine.Audio;
// Volumen general (Master del AudioMixer). Lo usan ScreenOptions, SoundManager y ScreenDeleteData.
public static class VolumeSettings
{
    private const string MixerParameter = "Master";
    public static float Load()
    {
        return PlayerPrefs.GetFloat(SaveKeys.MasterVolume, 1f);
    }
    public static void Save(float value)
    {
        PlayerPrefs.SetFloat(SaveKeys.MasterVolume, value);
    }
    public static void Apply(AudioMixer mixer, float value)
    {
        mixer.SetFloat(MixerParameter, Mathf.Log10(value) * 20);
    }
}
