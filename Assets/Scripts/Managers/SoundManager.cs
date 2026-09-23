using System.Collections.Generic;
using UnityEngine;
public enum SoundId
{
    GameStart, Jump, PlayerHurt, EnemyDeath, GameOver, Victory
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
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
    private AudioSource _audioSource;
    //cada SoundId esta relacionado con un AudioClip
    private Dictionary<SoundId, AudioClip> _clips = new Dictionary<SoundId, AudioClip>();
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        foreach (var sound in sounds)
        {
            _clips[sound.id] = sound.clip;
        }
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
    private void OnGameStart(params object[] parameters) => Play(SoundId.GameStart);
    private void OnPlayerJumped(params object[] parameters) => Play(SoundId.Jump);
    private void OnPlayerDamaged(params object[] parameters) => Play(SoundId.PlayerHurt);
    private void OnEnemyKilled(params object[] parameters) => Play(SoundId.EnemyDeath);
    private void OnGameOver(params object[] parameters) => Play(SoundId.GameOver);
    private void OnVictory(params object[] parameters) => Play(SoundId.Victory);
    public void Play(SoundId id)
    {
        if (!_clips.TryGetValue(id, out AudioClip clip) || clip == null) return;
        _audioSource.PlayOneShot(clip, volume);
    }
}
