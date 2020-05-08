[System.Serializable]
public class AdMobData
{
    public string appId;
    public SerializedStringArray adIds;
}

[System.Serializable]
public class SerializedStringArray
{
    public string[] stringArray;
}