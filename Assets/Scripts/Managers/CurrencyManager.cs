using UnityEngine;
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    private int _coins;
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
        _coins += amount;
        Save();
        NotifyUI();
    }
    // Para la tienda: devuelve false si no alcanza (igual que UseStamina)
    public bool SpendCoins(int amount)
    {
        if (_coins - amount < 0) return false;
        _coins -= amount;
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
        PlayerPrefs.SetInt(SaveKeys.Coins, _coins);
        PlayerPrefs.Save();
    }
    private void Load()
    {
        _coins = PlayerPrefs.GetInt(SaveKeys.Coins, 0);
    }
    private void NotifyUI()
    {
        EventManager.TriggerEvent(EventType.CoinsChanged, _coins);
    }
    public int Coins { get => _coins; }
}
