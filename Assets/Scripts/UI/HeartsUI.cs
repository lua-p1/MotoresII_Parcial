using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeartsUI : MonoBehaviour
{
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Color fullColor = Color.red;
    [SerializeField] private Color emptyColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    private List<Image> _hearts = new List<Image>();
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.PlayerHealthChanged, UpdateHearts); 
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.PlayerHealthChanged, UpdateHearts); 
    }
    private void UpdateHearts(params object[] parameters)  
    {
        float current = (float)parameters[0];              
        float max = (float)parameters[1];                  
        while (_hearts.Count < max)
        {
            _hearts.Add(Instantiate(heartPrefab, transform));
        }
        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].color = i < current ? fullColor : emptyColor;
        }
    }
}
