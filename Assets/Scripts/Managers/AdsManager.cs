using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager instance;
    [SerializeField] string _androidGameId = "800437922";
    [SerializeField] string _placementId = "BP_Interstitial_Android";
    [SerializeField] bool _testMode = true;                              
    private bool _adLoaded = false;                                      
    private void Awake()                                                
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        if (Advertisement.isInitialized)                 
            Advertisement.Load(_placementId, this);
        else
            Advertisement.Initialize(_androidGameId, _testMode, this);
    }
    public bool ShowAd()
    {
        if (!_adLoaded) return false;   // no habia anuncio listo
        _adLoaded = false;
        Advertisement.Show(_placementId, this);
        return true;                    // se pudo mostrar
    }
    #region//InitilizationInterface
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        Advertisement.Load(_placementId, this);
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    #endregion
    #region//LoadedInterface
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Unity Ads Ad Loaded: {placementId}");
        _adLoaded = true;                                
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Unity Ads Failed to Load: {placementId} - {error.ToString()} - {message}");
    }
    #endregion
    #region//ShowInterface
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Unity Ads Show Failure: {placementId} - {error.ToString()} - {message}");  
        Advertisement.Load(_placementId, this);                                                
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Unity Ads Show Complete: {placementId} - {showCompletionState.ToString()}");
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)      
            StaminaManager.instance.AddStamina(StaminaManager.instance.MaxStamina);
        else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED) 
            StaminaManager.instance.AddStamina(1);
        Advertisement.Load(_placementId, this);
    }
    public void OnUnityAdsShowStart(string placementId)
    {
    }
    public void OnUnityAdsShowClick(string placementId)
    {
    }
    #endregion
}