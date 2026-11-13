using UnityEngine;
public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public ShopItemData SelectedItem { get; private set; }
    public void SelectItem(ShopItemData item)
    {
        SelectedItem = item;
    }
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
    public bool CanBuy(IShopItem item)
    {
        return item.CanApply() && CurrencyManager.instance.Coins >= item.price; 
    }
    public bool TryBuy(IShopItem item)
    {
        if (!CanBuy(item)) return false;
        CurrencyManager.instance.SpendCoins(item.price);
        item.Apply();
        return true;
    }
}
