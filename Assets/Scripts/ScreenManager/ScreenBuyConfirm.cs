using TMPro;
using UnityEngine;
// Confirmacion de compra. ShopUI elige el item en ShopManager y abre esta pantalla.
// Cancelar usa BTN_Back (vuelve a la tienda).
public class ScreenBuyConfirm : ScreenBase
{
    [SerializeField] private TMP_Text questionText;
    public override void Activate()
    {
        base.Activate();   // SetActive(true) de ScreenBase
        var item = ShopManager.Instance.SelectedItem;
        questionText.text = $"Comprar {item.DisplayName} por {item.price} monedas?";
    }
    public void BTN_Confirm()
    {
        ShopManager.Instance.TryBuy(ShopManager.Instance.SelectedItem);
        BTN_Back();   // cierra la confirmacion y vuelve a la tienda
    }
}
