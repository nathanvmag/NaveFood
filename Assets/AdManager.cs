using UnityEngine;
using System.Collections;
using admob;
public class AdManager : MonoBehaviour {
    public static AdManager Instance { set; get; }
    string bannerID = "ca-app-pub-0245353620191711/7075999935";
    string videoID = "ca-app-pub-0245353620191711/8552733139";
	// Use this for initialization
	void Start () {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Admob.Instance().setTesting(true);
        Admob.Instance().initAdmob(bannerID, videoID);
        Admob.Instance().loadInterstitial();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ShowBanner()
    {
        Debug.Log("addd");
        Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 5, "BannerAd");
    }
    public void ShowVideo()
    {
        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
        }
    }
}
