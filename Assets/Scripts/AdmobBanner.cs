using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobBanner : MonoSingleton<AdmobBanner>
{
    private BannerView bannerView;
    [SerializeField] private string adUnitId;
    
    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void RequestBanner()
    {

        if (bannerView != null)
        {
            Debug.Log("Banner var");
            return;
        }
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    public void DestroyAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
