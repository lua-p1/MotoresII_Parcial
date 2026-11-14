using System;
using System.Collections;
using UnityEngine;
public class StaminaManager : MonoBehaviour
{
    public static StaminaManager instance;
    private int maxStamina = 3;
    private int currentStamina;
    private bool recharging = false;
    private DateTime nextStaminaTime;
    private Coroutine chargeCoroutine;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        chargeCoroutine = StartCoroutine(ChargeCoroutine());
        NotifyUI();
    }
    public bool IsFull()
    {
        return currentStamina >= maxStamina;
    }
    public bool UseStamina(int amount)
    {
        if (currentStamina - amount < 0) return false;
        currentStamina -= amount;
        if (!recharging && !IsFull())
        {
            nextStaminaTime = DateTime.Now.AddSeconds(RemoteConfigManager.SecondToRecover);
            chargeCoroutine = StartCoroutine(ChargeCoroutine());
        }
        Save();
        NotifyUI();
        return true;
    }
    public void AddStamina(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        if (recharging && IsFull())
        {
            StopCoroutine(chargeCoroutine);
            recharging = false;
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
        PlayerPrefs.SetInt("currentStamina", currentStamina);
        PlayerPrefs.SetString("nextStaminaTime", nextStaminaTime.ToString());
        PlayerPrefs.Save();
    }
    private void Load()
    {
        currentStamina = Mathf.Min(PlayerPrefs.GetInt("currentStamina", maxStamina), maxStamina);
        var dateTimeString = PlayerPrefs.GetString("nextStaminaTime");
        if (!string.IsNullOrEmpty(dateTimeString))
            nextStaminaTime = DateTime.Parse(dateTimeString);
    }
    void NotifyUI()
    {
        TimeSpan timer = nextStaminaTime - DateTime.Now;
        EventManager.TriggerEvent(EventType.StaminaChanged, currentStamina, maxStamina, timer);
    }
    IEnumerator ChargeCoroutine()
    {
        recharging = true;
        while (!IsFull())
        {
            DateTime now = DateTime.Now;
            DateTime nextStamina = nextStaminaTime;
            bool staminaAdded = false;
            while (now > nextStamina)
            {
                if (IsFull()) break;
                staminaAdded = true;
                nextStamina = nextStamina.AddSeconds(RemoteConfigManager.SecondToRecover);
                currentStamina++;
            }
            if (staminaAdded)
            {
                nextStaminaTime = nextStamina;
                Save();
            }
            NotifyUI();
            yield return new WaitForSeconds(1f);
        }
        recharging = false;
    }
    public int MaxStamina { get => maxStamina; }
}
