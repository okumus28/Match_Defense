using UnityEngine;

public class GameSceneScripts : MonoBehaviour
{
    private void OnEnable()
    {
        GameSignals.OnGameStarting?.Invoke();
    }

    private void Start()
    {
        AdmobBanner.Instance.RequestBanner();
        
    }
}
