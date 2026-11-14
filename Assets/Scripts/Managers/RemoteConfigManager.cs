using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
public class RemoteConfigManager : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }
    async void Start()
    {
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfig();
        }
        RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());

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
