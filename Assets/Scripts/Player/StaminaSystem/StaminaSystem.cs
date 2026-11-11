using System;
using System.Collections;
using UnityEngine;
public class StaminaSystem : MonoBehaviour
{
    [SerializeField] int maxStamina = 3;
    [SerializeField] float secondToRecover = 5f;
    int currentStamina;
    bool recharging = false;
    DateTime nextStamineTime;
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

    public bool UseStamine(int amount)
    {
        if (currentStamina - amount < 0) return false;
        currentStamina -= amount;
        if (!recharging && currentStamina < maxStamina)
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
        if (recharging && currentStamina >= maxStamina)
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
        PlayerPrefs.SetString("nextStaminaTime", nextStamineTime.Ticks.ToString());
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
        while (currentStamina < maxStamina)
        {
            DateTime now = DateTime.Now;
            DateTime nextStamine = nextStamineTime;
            bool staminedAdded = false;
            while (now > nextStamine)
            {
                if (currentStamina >= maxStamina) break;
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
}
