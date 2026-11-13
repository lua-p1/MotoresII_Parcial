using UnityEngine;
[CreateAssetMenu(fileName = "ExtraHeart", menuName = "Shop/Extra Heart")]
public class ExtraHeartItem : ShopItemData
{
    private const string PendingKey = "extraHeartPending";
    public override bool CanApply() => !IsPending();
    public override void Apply()
    {
        PlayerPrefs.SetInt(PendingKey, 1);
        PlayerPrefs.Save();
    }
    private static bool IsPending()
    {
        return PlayerPrefs.GetInt(PendingKey, 0) == 1;
    }
    public static bool Consume()
    {
        if (!IsPending()) return false;
        PlayerPrefs.SetInt(PendingKey, 0);
        PlayerPrefs.Save();
        return true;
    }
}
