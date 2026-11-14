using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum SoundId
{
    GameStart, Jump, PlayerHurt, EnemyDeath, GameOver, Victory
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour , IPauseable
{
    [System.Serializable]
    private struct SoundEntry
    {
        public SoundId id;
        public AudioClip clip;
    }
    [SerializeField] private List<SoundEntry> sounds;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;
    [Header("Music")]
    [SerializeField] private AudioClip musicClip;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.4f;
    [Range(0f, 1f)]
    [SerializeField] private float pausedMusicMultiplier = 0.5f;
    [Header("Mixer")]                                          
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    private AudioSource _sfxSource;
    private AudioSource _musicSource;

    //cada SoundId esta relacionado con un AudioClip
    private Dictionary<SoundId, AudioClip> _clips = new Dictionary<SoundId, AudioClip>();
    private void Awake()
    {
        _sfxSource = GetComponent<AudioSource>();
        _sfxSource.playOnAwake = false;
        _sfxSource.outputAudioMixerGroup = sfxGroup;
        foreach (var sound in sounds)
        {
            _clips[sound.id] = sound.clip;
        }
        //AudioSource aparte para la musica, asi no comparte volumen con los efectos
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.clip = musicClip;
        _musicSource.loop = true;
        _musicSource.volume = musicVolume;
        _musicSource.outputAudioMixerGroup = musicGroup;
    }
    private void Start()
    {
        // Aplica el volumen guardado apenas arranca el nivel
        if (musicGroup != null)
            VolumeSettings.Apply(musicGroup.audioMixer, VolumeSettings.Load());
        if (musicClip != null) _musicSource.Play();
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.GameStart, OnGameStart);
        EventManager.SubscribeToEvent(EventType.PlayerJumped, OnPlayerJumped);
        EventManager.SubscribeToEvent(EventType.PlayerDamaged, OnPlayerDamaged);
        EventManager.SubscribeToEvent(EventType.EnemyKilled, OnEnemyKilled);
        EventManager.SubscribeToEvent(EventType.GameOver, OnGameOver);
        EventManager.SubscribeToEvent(EventType.LevelComplete, OnVictory);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.GameStart, OnGameStart);
        EventManager.UnsubscribeToEvent(EventType.PlayerJumped, OnPlayerJumped);
        EventManager.UnsubscribeToEvent(EventType.PlayerDamaged, OnPlayerDamaged);
        EventManager.UnsubscribeToEvent(EventType.EnemyKilled, OnEnemyKilled);
        EventManager.UnsubscribeToEvent(EventType.GameOver, OnGameOver);
        EventManager.UnsubscribeToEvent(EventType.LevelComplete, OnVictory);
    }
    private void OnGameStart(params object[] parameters) => Play(SoundId.GameStart);//Agregar Sonido
    private void OnPlayerJumped(params object[] parameters) => Play(SoundId.Jump);
    private void OnPlayerDamaged(params object[] parameters) => Play(SoundId.PlayerHurt);
    private void OnEnemyKilled(params object[] parameters) => Play(SoundId.EnemyDeath);
    private void OnGameOver(params object[] parameters)
    {
        _musicSource.Stop();
        Play(SoundId.GameOver);
    }
    private void OnVictory(params object[] parameters)
    {
        _musicSource.Stop();
        Play(SoundId.Victory);
    }
    public void Play(SoundId id)
    {
        if (!_clips.TryGetValue(id, out AudioClip clip) || clip == null) return;
        _sfxSource.PlayOneShot(clip, volume);
    }
    public void Pause()
    {
        _musicSource.volume = musicVolume * pausedMusicMultiplier;
    }
    public void Resume()
    {
        _musicSource.volume = musicVolume;
    }
}
