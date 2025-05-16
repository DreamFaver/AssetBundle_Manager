using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PinkFish_AssetLoader : MonoBehaviour
{
    public enum Platform { Windows, Android, WebGL, IOS }

    public Platform platform;
    public string BundlePath;

    private List<AssetBundle> loadedBundles = new List<AssetBundle>();
    private string[] assetNames;

    public List<string> LoadedAsset = new List<string>();       // All asset names inside bundles
    public List<string> LoadedBundles = new List<string>();     // Loaded bundle file names

    // Load all AssetBundles in platform folder and populate lists
    public void LoadAssetBundles()
    {
        BundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundle", GetPlatformFolderName());

        if (!Directory.Exists(BundlePath))
        {
            Debug.LogWarning($"AssetBundle directory not found at: {BundlePath}");
            return;
        }

        string[] bundleFiles = Directory.GetFiles(BundlePath, "*.*", SearchOption.TopDirectoryOnly);

        if (bundleFiles.Length == 0)
        {
            Debug.LogWarning($"No asset bundle files found in: {BundlePath}");
            return;
        }

        LoadedAsset.Clear();
        LoadedBundles.Clear();
        loadedBundles.Clear();

        foreach (var file in bundleFiles)
        {
            string ext = Path.GetExtension(file).ToLower();
            if (ext == ".manifest" || ext == ".meta") continue;

            AssetBundle ab = AssetBundle.LoadFromFile(file);
            if (ab == null)
            {
                Debug.LogWarning($"Failed to load AssetBundle: {file}");
                continue;
            }

            loadedBundles.Add(ab);
            LoadedBundles.Add(Path.GetFileName(file));

            assetNames = ab.GetAllAssetNames();
            foreach (string name in assetNames)
            {
                LoadedAsset.Add(name);
            }
        }

        Debug.Log($"Loaded {LoadedBundles.Count} bundles with {LoadedAsset.Count} total assets.");
    }

    // Unload all loaded AssetBundles
    public void UnloadAssetBundles(bool unloadAllLoadedObjects = false)
    {
        foreach (var ab in loadedBundles)
        {
            ab.Unload(unloadAllLoadedObjects);
        }

        loadedBundles.Clear();
        LoadedAsset.Clear();
        LoadedBundles.Clear();

        Debug.Log("All AssetBundles unloaded.");
    }

    // Helper to get folder name based on enum
    private string GetPlatformFolderName()
    {
        switch (platform)
        {
            case Platform.Windows: return "StandaloneWindows64";
            case Platform.Android: return "Android";
            case Platform.WebGL: return "WebGL";
            case Platform.IOS: return "IOS";
            default: return "StandaloneWindows64";
        }
    }
}
