using UnityEngine;
public class ExtraHeartItem : MonoBehaviour, IShopItem
{
    public int price { get; private set; } = 150;
    private const string PendingKey = "extraHeartPending";
    public bool CanApply() => !IsPending();
    public void Apply()
    {
        PlayerPrefs.SetInt(PendingKey, 1);
        PlayerPrefs.Save();
    }
    private static bool IsPending()
    {
        return PlayerPrefs.GetInt(PendingKey,0) == 1;
    }
    public static bool Consume()
    {
        if (!IsPending()) return false;
        PlayerPrefs.SetInt(PendingKey, 0);
        PlayerPrefs.Save();
        return true;
    }
}
