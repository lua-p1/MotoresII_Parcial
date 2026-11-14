using UnityEngine;
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    private int coins;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(EventType.LevelComplete, OnLevelComplete);
    }
    private void OnDisable()
    {
        EventManager.UnsubscribeToEvent(EventType.LevelComplete, OnLevelComplete);
    }
    private void Start()
    {
        Load();
        NotifyUI();
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        Save();
        NotifyUI();
    }
    // Para la tienda: devuelve false si no alcanza (igual que UseStamina)
    public bool SpendCoins(int amount)
    {
        if (coins - amount < 0) return false;
        coins -= amount;
        Save();
        NotifyUI();
        return true;
    }
    private void OnLevelComplete(params object[] parameters)
    {
        AddCoins(RemoteConfigManager.CoinsPerWin);
    }
    private void Save()
    {
        PlayerPrefs.SetInt(SaveKeys.Coins, coins);
        PlayerPrefs.Save();
    }
    private void Load()
    {
        coins = PlayerPrefs.GetInt(SaveKeys.Coins, 0);
    }
    private void NotifyUI()
    {
        EventManager.TriggerEvent(EventType.CoinsChanged, coins);
    }
    public int Coins { get => coins; }
}
