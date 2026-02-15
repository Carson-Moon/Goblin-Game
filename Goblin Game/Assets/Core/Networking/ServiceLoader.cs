using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;

public class ServiceLoader : MonoBehaviour
{
    void Awake()
    {
        UsernameHolder.FetchExistingUsername();
    }

    private async void Start() 
    {
        await InitializeAsync();
    }

    async Task InitializeAsync()
    {
        try
        {
            Debug.Log("Initializing Unity Services...");
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services initialized.");

            Debug.Log("Signing in...");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in.");

            Debug.Log("Initializing Vivox...");
            await VivoxService.Instance.InitializeAsync();
            Debug.Log("Vivox initialized.");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
}
