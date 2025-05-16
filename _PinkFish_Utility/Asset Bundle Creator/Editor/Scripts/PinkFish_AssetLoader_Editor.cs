using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(PinkFish_AssetLoader))]
public class PinkFish_AssetLoader_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Load AssetBundles", GUILayout.Width(150), GUILayout.Height(30)))
        {
            var loader = (PinkFish_AssetLoader)target;
            var method = typeof(PinkFish_AssetLoader).GetMethod("LoadAssetBundles", BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(loader, null);
                Debug.Log("LoadAssetBundles() executed.");
            }
            else
            {
                Debug.LogError("LoadAssetBundles() method not found.");
            }
        }

        if (GUILayout.Button("Unload AssetBundles", GUILayout.Width(150), GUILayout.Height(30)))
        {
            var loader = (PinkFish_AssetLoader)target;
            var method = typeof(PinkFish_AssetLoader).GetMethod("UnloadAssetBundles", BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(loader, new object[] { false }); // Default: don't unload loaded assets
                Debug.Log("UnloadAssetBundles() executed.");
            }
            else
            {
                Debug.LogError("UnloadAssetBundles() method not found.");
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
