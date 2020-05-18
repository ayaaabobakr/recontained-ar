using System.Collections;
using UnityEditor;
using UnityEngine;

public class createAssetBundle : MonoBehaviour {
    [MenuItem ("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles () {
        BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}