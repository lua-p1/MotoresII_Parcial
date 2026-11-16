using System;
using System.Collections;
using UnityEngine;
public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance { get; private set; }
    private int _maxStamina = 3;
    private int _currentStamina;
    private bool _recharging = false;
    private DateTime _nextStaminaTime;
    private Coroutine _chargeCoroutine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.LevelComplete, OnLevelComplete);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.LevelComplete, OnLevelComplete);
    }
    private void Start()
    {
        Load();
        _chargeCoroutine = StartCoroutine(ChargeCoroutine());
        NotifyUI();
    }
    public bool IsFull()
    {
        return _currentStamina >= _maxStamina;
    }
    public bool UseStamina(int amount)
    {
        if (_currentStamina - amount < 0) return false;
        _currentStamina -= amount;
        if (!_recharging && !IsFull())
        {
            _nextStaminaTime = DateTime.Now.AddSeconds(RemoteConfigManager.SecondsToRecover);
            _chargeCoroutine = StartCoroutine(ChargeCoroutine());
        }
        Save();
        NotifyUI();
        return true;
    }
    public void AddStamina(int amount)
    {
        _currentStamina = Mathf.Min(_currentStamina + amount, _maxStamina);
        if (_recharging && IsFull())
        {
            StopCoroutine(_chargeCoroutine);
            _recharging = false;
        }
        Save();     
        NotifyUI();
    }
    private void OnLevelComplete(params object[] parameters)
    {
        if (RemoteConfigManager.RefundStaminaOnWin)
            AddStamina(1);
    }
    private void Save()
    {
        PlayerPrefs.SetInt(SaveKeys.CurrentStamina, _currentStamina);
        PlayerPrefs.SetString(SaveKeys.NextStaminaTime, _nextStaminaTime.ToString());
        PlayerPrefs.Save();
    }
    private void Load()
    {
        _currentStamina = Mathf.Min(PlayerPrefs.GetInt(SaveKeys.CurrentStamina, _maxStamina), _maxStamina);
        var dateTimeString = PlayerPrefs.GetString(SaveKeys.NextStaminaTime);
        if (!string.IsNullOrEmpty(dateTimeString))
            _nextStaminaTime = DateTime.Parse(dateTimeString);
    }
    private void NotifyUI()
    {
        TimeSpan timer = _nextStaminaTime - DateTime.Now;
        EventManager.TriggerEvent(EventType.StaminaChanged, _currentStamina, _maxStamina, timer);
    }
    private IEnumerator ChargeCoroutine()
    {
        _recharging = true;
        while (!IsFull())
        {
            DateTime now = DateTime.Now;
            DateTime nextStamina = _nextStaminaTime;
            bool staminaAdded = false;
            while (now > nextStamina)
            {
                if (IsFull()) break;
                staminaAdded = true;
                nextStamina = nextStamina.AddSeconds(RemoteConfigManager.SecondsToRecover);
                _currentStamina++;
            }
            if (staminaAdded)
            {
                _nextStaminaTime = nextStamina;
                Save();
            }
            NotifyUI();
            yield return new WaitForSeconds(1f);
        }
        _recharging = false;
    }
    public int MaxStamina { get => _maxStamina; }
}
