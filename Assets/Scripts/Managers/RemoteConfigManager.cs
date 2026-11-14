using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
public class RemoteConfigManager : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }
    public static int CoinsPerWin => RemoteConfigService.Instance.appConfig.GetInt("coinsPerWin", 50);
    public static float SecondToRecover => RemoteConfigService.Instance.appConfig.GetFloat("secondToRecover", 300f);
    public static int AdCompletedReward => RemoteConfigService.Instance.appConfig.GetInt("adCompletedReward", 3);
    public static int AdSkippedReward => RemoteConfigService.Instance.appConfig.GetInt("adSkippedReward", 1);
    public static bool RefundStaminaOnWin => RemoteConfigService.Instance.appConfig.GetBool("refundStaminaOnWin", true);
    private async void Start()
    {
        try
        {
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfig();
            }
            RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }
        catch(Exception e)
        {
            Debug.LogWarning($"Remote Config no disponible, se usan los valores por defecto: {e.Message}");
        }


    }
    private async Task InitializeRemoteConfig()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    private void OnFetchCompleted(ConfigResponse configResponse)
    {
        Debug.Log("Remote Config Fetch Completed");
        Debug.Log(RemoteConfigService.Instance.appConfig.config);
    }
    private void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;
    }
}
