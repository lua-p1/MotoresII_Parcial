using UnityEngine;
public class EnergyRefillItem : MonoBehaviour, IShopItem
{
    [SerializeField] private int price = 75;
    public int GetPrice() => price;
    public bool CanApply() => !StaminaManager.instance.IsFull();
    public void Apply()
    {
        StaminaManager.instance.AddStamine(StaminaManager.instance.MaxStamina);
    }
}
