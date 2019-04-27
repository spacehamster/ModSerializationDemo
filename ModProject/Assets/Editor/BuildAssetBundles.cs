using GameLib;
using ModLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundles
{

    [MenuItem("Assets/Build Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "../AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        var gameObject = AssetDatabase.LoadAssetAtPath<GameScriptableObject>("Assets/ScriptableObjects/TestGameObject.asset");
        var derivedObject = AssetDatabase.LoadAssetAtPath<DerivedScriptableObject>("Assets/ScriptableObjects/TestDerivedObject.asset");
        //Save derived object and mod object as text here
        //Save list of all field references to derived object here
        //Remove field references to derived object so that derived object isn't in asset bundle
        gameObject.SO = null;
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
        //Restore field references here
        gameObject.SO = derivedObject;
    }
    [MenuItem("Assets/Build Manual Asset Bundles")]
    static void BuildAllManualAssetBundle()
    {
        string assetBundleDirectory = "../AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        //Alternatively, manually build the object graph, and leave mod objects out
        //Save derived object as text here
        //Save list of all field references here
        var gameObject = AssetDatabase.LoadAssetAtPath<GameScriptableObject>("Assets/ScriptableObjects/TestGameObject.asset");
        var monoscript = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/GameScripts/GameScriptableObject.cs");
        BuildPipeline.BuildAssetBundle(gameObject, new Object[] { monoscript }, 
            $"{assetBundleDirectory}/manual_bundle", BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);

    }
}

