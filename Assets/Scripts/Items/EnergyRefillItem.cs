using UnityEngine;
[CreateAssetMenu(fileName = "EnergyRefill", menuName = "Shop/Energy Refill")]
public class EnergyRefillItem : ShopItemData
{
    public override bool CanApply() => !StaminaManager.Instance.IsFull();
    public override void Apply()
    {
        StaminaManager.Instance.AddStamina(StaminaManager.Instance.MaxStamina);
    }
}
