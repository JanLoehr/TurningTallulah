using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;

public class BannerAd : MonoBehaviour
{
    public string bannerPlacement = "Banner";
    public bool TestMode = true;


    // if you checked this out via git, you need to fill in dummy strings here or ask for my appIDs ;)
#if UNITY_ANDROID
    string appId = GitIgnoreThis_Constants.AndroidAdId;
#elif UNITY_IPHONE
    string appId = GitIgnoreThis_Constants.iOSAdId;
#else
    string appId = "unexpected_platform";
#endif

    public void Start()
    {
        Advertisement.Initialize(appId, TestMode);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        RequestBanner();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Advertisement.Banner.Hide(true);
    }

    private void RequestBanner()
    {
        StartCoroutine(ShowAdWhenReady());
    }

    private IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady(bannerPlacement))
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.Show(bannerPlacement);
    }
}