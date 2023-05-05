using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{
    private void Start()
    {
        AdmobBanner.Instance.RequestBanner();
        //AdsManager.Instance.BannerAdShow();        
    }
    public void StartButton()
    {
        //Debug.Log("asdasda");
        SceneManager.LoadScene(1);
        AdmobBanner.Instance.DestroyAd();
    }
}
