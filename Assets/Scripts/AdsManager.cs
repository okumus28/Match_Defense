using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdsManager : MonoBehaviour , IUnityAdsShowListener , IUnityAdsLoadListener, IUnityAdsInitializationListener
{
#if UNITY_ANDROID
    string gameId = "5265601";
    string rewardedAdID = "Rewarded_Android";
    string insterstitalAdID = "Interstitial_Android";
    string bannerAdID = "Banner_Android";
#endif
    // Start is called before the first frame update

    Action OnRewardedAdSuccess;
    void Start()
    {
        Advertisement.Initialize(gameId , false , this);
        Advertisement.Load(rewardedAdID, this);
        Advertisement.Load(insterstitalAdID, this);
        Advertisement.Load(bannerAdID, this);

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        //DontDestroyOnLoad(this);
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Error : " + message);
    }


    public void PlayRewardedAd(Action success)
    {
        OnRewardedAdSuccess = success;
        Advertisement.Show(rewardedAdID , this);
    }

    public void InterstitalAd()
    {
        Advertisement.Show(insterstitalAdID , this);

    }

    public void BannerAdShow()
    {
        Advertisement.Banner.Show(bannerAdID );
    }
    
    public void BannerAdHide()
    {
        Advertisement.Banner.Hide();

    }







    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Show Rewarded");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == rewardedAdID && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            OnRewardedAdSuccess.Invoke();
            Debug.Log("ÖDÜL VERİLDİ............!");
        }
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {

        if (placementId.Equals(rewardedAdID))
        {
            Debug.Log("Ads Loaded : " + placementId);
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Ads Load Error : " + message);
    }
}
