using UnityEngine;
public class EnergyRefillItem : MonoBehaviour, IShopItem
{
    public int price { get; private set; } = 75;
    public bool CanApply() => !StaminaManager.instance.IsFull();
    public void Apply()
    {
        StaminaManager.instance.AddStamine(StaminaManager.instance.MaxStamina);
    }
}
