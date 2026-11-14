using UnityEngine;
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    public ShopItemData SelectedItem { get; private set; }
    public void SelectItem(ShopItemData item)
    {
        SelectedItem = item;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool CanBuy(IShopItem item)
    {
        return item.CanApply() && CurrencyManager.Instance.Coins >= item.Price; 
    }
    public bool TryBuy(IShopItem item)
    {
        if (!CanBuy(item)) return false;
        CurrencyManager.Instance.SpendCoins(item.Price);
        item.Apply();
        return true;
    }
}
