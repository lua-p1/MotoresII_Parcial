using TMPro;
using UnityEngine;
public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.CoinsChanged, UpdateUI);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.CoinsChanged, UpdateUI);
    }
    private void UpdateUI(params object[] parameters)
    {
        int coins = (int)parameters[0];
        coinsText.text = coins.ToString();
    }
}
