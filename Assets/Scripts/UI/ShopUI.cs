using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopItemData item;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject lockedOverlay;
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
        if (!ShopManager.instance.CanBuy(item))
        {
            // Si el item no se puede aplicar, su popup; si no, el problema son las monedas
            string errorScreen = item.CanApply() ? "NoCoinsScreen" : item.CantApplyScreen;
            ScreenManager.Instance.Push(errorScreen);
            return;
        }
        ShopManager.instance.SelectItem(item);
        ScreenManager.Instance.Push("BuyConfirmScreen");
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
        iconImage.sprite = item.Icon;
        nameText.text = item.DisplayName;
        descriptionText.text = item.Description;
        priceText.text = item.price.ToString();
        buyButton.interactable = true;
        priceText.alpha = ShopManager.instance.CanBuy(item) ? 1f : 0.4f;
    }
}
