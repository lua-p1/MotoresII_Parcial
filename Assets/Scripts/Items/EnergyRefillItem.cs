using UnityEngine;
[CreateAssetMenu(fileName = "EnergyRefill", menuName = "Shop/Energy Refill")]
public class EnergyRefillItem : ShopItemData
{
    public override bool CanApply() => !StaminaManager.instance.IsFull();
    public override void Apply()
    {
        StaminaManager.instance.AddStamina(StaminaManager.instance.MaxStamina);
    }
}
