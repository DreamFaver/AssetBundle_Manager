using UnityEditor;
using UnityEngine;
using System.IO;

public class PinkFish_AssetBuilder : EditorWindow
{
    private Texture2D logo;

    private readonly string basePath = "Assets/StreamingAssets/AssetBundle/";

    [MenuItem("Pink Fish Utility/AssetBundle/Creator")]
    public static void ShowWindow()
    {
        var window = GetWindow<PinkFish_AssetBuilder>("Pink Fish Utility");
        window.minSize = new Vector2(350, 230);
        window.maxSize = new Vector2(350, 230);
        window.LoadLogos();
    }

    private void LoadLogos()
    {
        logo = Resources.Load<Texture2D>("Logo/Pink_Fish_logo_fish");
        if (logo == null)
        {
            Debug.LogWarning("Pink Fish Utility logo not found at Resources/Logo/Pink_Fish_logo_fish.png");
        }
    }

    private void OnGUI()
    {
        DrawTitle();

        GUILayout.Space(10);

        DrawPlatformButton("Windows", BuildTarget.StandaloneWindows64, "StandaloneWindows64");
        DrawPlatformButton("Android", BuildTarget.Android, "Android");
        DrawPlatformButton("WebGL", BuildTarget.WebGL, "WebGL");
        DrawPlatformButton("iOS", BuildTarget.iOS, "iOS");
    }

    private void DrawTitle()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (logo != null)
            GUILayout.Label(logo, GUILayout.Width(40), GUILayout.Height(40));
        else
            GUILayout.Space(40);

        GUILayout.Label("<b><size=18>Asset Bundle Creator</size></b>", new GUIStyle(EditorStyles.label)
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fixedHeight = 40,
            stretchWidth = true
        }, GUILayout.ExpandWidth(true));

        if (logo != null)
            GUILayout.Label(logo, GUILayout.Width(40), GUILayout.Height(40));
        else
            GUILayout.Space(40);

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawPlatformButton(string displayName, BuildTarget buildTarget, string folderName)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        float totalWidth = 320;
        float buttonWidth = totalWidth * 0.75f;
        float clearButtonWidth = 30;

        if (GUILayout.Button($"Create {displayName} Asset Bundle", GUILayout.Width(buttonWidth), GUILayout.Height(30)))
        {
            BuildAssetBundle(buildTarget, folderName);
        }

        if (GUILayout.Button("X", GUILayout.Width(clearButtonWidth), GUILayout.Height(30)))
        {
            ClearAssetBundleFolder(folderName);
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
    }

    private void BuildAssetBundle(BuildTarget target, string folderName)
    {
        string path = basePath + folderName;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        ClearAssetBundleFolder(folderName);

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, target);

        AssetDatabase.Refresh();
    }

    private void ClearAssetBundleFolder(string folderName)
    {
        string path = basePath + folderName;
        if (!Directory.Exists(path))
        {
            Debug.LogWarning($"[Pink Fish] Folder not found: {path}");
            return;
        }

        string[] files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;
            File.Delete(file);
        }

        Debug.Log($"[Pink Fish] Cleared asset bundle folder: {path}");
        AssetDatabase.Refresh();
    }
}
