using UnityEngine;
// Base de todos los items de la tienda: datos que se editan en el asset
// + el comportamiento que cada item concreto implementa.
public abstract class ShopItemData : ScriptableObject, IShopItem
{
    public string DisplayName { get; private set; }
    public string Description { get; private set; }
    public Sprite Icon { get; private set; }
    public int price { get; private set; }
    public abstract bool CanApply();
    public abstract void Apply();
}
