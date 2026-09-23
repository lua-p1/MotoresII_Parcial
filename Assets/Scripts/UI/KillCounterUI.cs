using TMPro;
using UnityEngine;
public class KillCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.EnemyCountChanged, UpdateCounter);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.EnemyCountChanged, UpdateCounter);
    }
    private void UpdateCounter(params object[] parameters)
    {
        counterText.text = $"{parameters[0]} / {parameters[1]}";
    }
}
