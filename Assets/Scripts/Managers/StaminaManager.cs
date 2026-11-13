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
        StartCoroutine(ChargeCorutine());
        NotifyUI();
    }
    public bool IsFull()
    {
        return currentStamina >= maxStamina;
    }
    public bool UseStamine(int amount)
    {
        if (currentStamina - amount < 0) return false;
        currentStamina -= amount;
        if (!recharging && !IsFull())
        {
            nextStamineTime = DateTime.Now.AddSeconds(secondToRecover);
            StartCoroutine(ChargeCorutine());
        }
        Save();
        NotifyUI();
        return true;
    }
    public void AddStamine(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        if (recharging && IsFull())
        {
            StopAllCoroutines();
            recharging = false;
        }
        Save();     
        NotifyUI();
    }
    private void OnLevelComplete(params object[] parameters)
    {
        AddStamine(1);
    }
    public void Save()
    {
        PlayerPrefs.SetInt("currentStamina", currentStamina);
        PlayerPrefs.SetString("nextStaminaTime", nextStamineTime.ToString());
        PlayerPrefs.Save();
    }
    public void Load()
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
    IEnumerator ChargeCorutine()
    {
        recharging = true;
        while (!IsFull())
        {
            DateTime now = DateTime.Now;
            DateTime nextStamine = nextStamineTime;
            bool staminedAdded = false;
            while (now > nextStamine)
            {
                if (IsFull()) break;
                staminedAdded = true;
                nextStamine = nextStamine.AddSeconds(secondToRecover);
                currentStamina++;
            }
            if (staminedAdded)
            {
                nextStamineTime = nextStamine;
                Save();
            }
            NotifyUI();
            yield return null;
        }
        recharging = false;
    }
    public int MaxStamina { get => maxStamina; }
}
