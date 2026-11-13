using System;
using System.Collections;
using UnityEngine;
public class StaminaManager : MonoBehaviour
{
    public static StaminaManager instance;
    private int maxStamina = 3;
    private float secondToRecover = 300f;
    private int currentStamina;
    private bool recharging = false;
    private DateTime nextStamineTime;
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
            nextStamineTime = DateTime.Now.AddSeconds(secondToRecover);
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
        AddStamina(1);
    }
    private void Save()
    {
        PlayerPrefs.SetInt("currentStamina", currentStamina);
        PlayerPrefs.SetString("nextStaminaTime", nextStamineTime.ToString());
        PlayerPrefs.Save();
    }
    private void Load()
    {
        currentStamina = Mathf.Min(PlayerPrefs.GetInt("currentStamina", maxStamina), maxStamina);
        var dateTimeString = PlayerPrefs.GetString("nextStaminaTime");
        if (!string.IsNullOrEmpty(dateTimeString))
            nextStamineTime = DateTime.Parse(dateTimeString);
    }
    void NotifyUI()
    {
        TimeSpan timer = nextStamineTime - DateTime.Now;
        EventManager.TriggerEvent(EventType.StaminaChanged, currentStamina, maxStamina, timer);
    }
    IEnumerator ChargeCoroutine()
    {
        recharging = true;
        while (!IsFull())
        {
            DateTime now = DateTime.Now;
            DateTime nextStamine = nextStamineTime;
            bool staminaAdded = false;
            while (now > nextStamine)
            {
                if (IsFull()) break;
                staminaAdded = true;
                nextStamine = nextStamine.AddSeconds(secondToRecover);
                currentStamina++;
            }
            if (staminaAdded)
            {
                nextStamineTime = nextStamine;
                Save();
            }
            NotifyUI();
            yield return new WaitForSeconds(1f);
        }
        recharging = false;
    }
    public int MaxStamina { get => maxStamina; }
}
