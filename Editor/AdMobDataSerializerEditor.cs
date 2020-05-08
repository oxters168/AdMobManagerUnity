using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(AdMobDataSerializer))]
public class AdMobDataSerializerEditor : Editor
{
    public string saveLoc = "Assets/Resources/AdMobData/AdIds.json";
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        
        var admobData = ((AdMobDataSerializer)target).admobData;
        if (GUILayout.Button("Serialize"))
        {
            string jsonData = JsonUtility.ToJson(admobData);

            using (FileStream fs = new FileStream(saveLoc, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fs))
                    writer.Write(jsonData);
                    
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
            #endif
        }
    }
}