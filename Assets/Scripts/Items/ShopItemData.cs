using UnityEngine;
// Base de todos los items de la tienda: datos que se editan en el asset
// + el comportamiento que cada item concreto implementa.
public abstract class ShopItemData : ScriptableObject, IShopItem
{
    [SerializeField] private string displayName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int itemPrice;
    [SerializeField] private string cantApplyScreen;
    public string DisplayName { get => displayName; }   
    public string Description { get => description; }   
    public Sprite Icon { get => icon; }
    public int price { get => itemPrice; }
    public string CantApplyScreen { get => cantApplyScreen; }
    public abstract bool CanApply();
    public abstract void Apply();
}
