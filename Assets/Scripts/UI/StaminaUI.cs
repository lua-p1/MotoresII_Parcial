using System;
using TMPro;
using UnityEngine;
public class StaminaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text timerText;
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.StaminaChanged, UpdateUI);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.StaminaChanged, UpdateUI);
    }
    void UpdateUI(params object[] parameters)
    {
        int currentStamina = (int)parameters[0];
        int maxStamina = (int)parameters[1];
        TimeSpan timer = (TimeSpan)parameters[2];
        staminaText.text = currentStamina + "/" + maxStamina;
        if (currentStamina >= maxStamina)
        {
            timerText.text = "Full";
            return;
        }
        timerText.text = timer.Minutes.ToString("00") + ":" + timer.Seconds.ToString("00");
    }
}
