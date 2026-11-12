using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopUI : MonoBehaviour
{

    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject lockedOverlay;
    private IShopItem item;
    private void Awake()
    {
        item = GetComponent<IShopItem>();
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.CoinsChanged, OnDataChanged);
        EventManager.SubscribeToEvent(EventType.StaminaChanged, OnDataChanged);
        Refresh();
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.CoinsChanged, OnDataChanged);
        EventManager.UnsubscribeToEvent(EventType.StaminaChanged, OnDataChanged);
    }
    private void OnDataChanged(params object[] parameters)
    {
        Refresh();
    }
    public void Buy()
    {
        ShopManager.instance.TryBuy(item);
        Refresh();
    }
    private void Refresh()
    {
        bool locked = item == null;
        lockedOverlay.SetActive(locked);
        if (locked)
        {
            priceText.text = "-";
            buyButton.interactable = false;
            return;
        }
        priceText.text = item.GetPrice().ToString();
        buyButton.interactable = ShopManager.instance.CanBuy(item);
    }
}
