using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobController : MonoBehaviour
{
    private static AdMobController admobControllerInScene;

    [Tooltip("If set to true, will retrieve ad ids from Json file located in 'Assets/Resources/AdMobData/AdIds.json'")]
    public bool fromJson;
    public string appId;
    public bool testAds = true;

    public AdUnit[] adUnits;

    private void Awake()
    {
        admobControllerInScene = this;
    }
    private void Start()
    {
        if (testAds)
        {
            #if UNITY_ANDROID
                appId = "ca-app-pub-3940256099942544~3347511713";
            #elif UNITY_IPHONE
                appId = "ca-app-pub-3940256099942544~1458002511";
            #else
                appId = "unexpected_platform";
            #endif
        }
        else if (fromJson)
        {
            var jsonTextFile = Resources.Load<TextAsset>("AdMobData/AdIds");
            AdMobData admobData = JsonUtility.FromJson<AdMobData>(jsonTextFile.text);
            appId = admobData.appId;
            for (int i = 0; i < adUnits.Length; i++)
                adUnits[i].adId = admobData.adIds.stringArray[i];
        }

        MobileAds.Initialize(appId);

        foreach (AdUnit adUnit in adUnits)
            adUnit.Initialize(testAds);
    }

    public static void ShowAd(int index)
    {
        admobControllerInScene.adUnits[index].Show();
    }
    public static void HideAd(int index)
    {
        admobControllerInScene.adUnits[index].Hide();
    }
}