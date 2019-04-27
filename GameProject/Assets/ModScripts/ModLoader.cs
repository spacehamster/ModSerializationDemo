using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class ModLoader : MonoBehaviour {
    void DeserializeModObjectsFromFile(List<Object> bundleObjects)
    {
        var gameObject = bundleObjects.First();
        //Load DerivedObject and ModObject from text file here
        var derivedType = Type.GetType("ModLib.DerivedScriptableObject, ModLib");
        var modType = Type.GetType("ModLib.ModScriptableObject, ModLib");
        var modPOCType = Type.GetType("ModLib.ModPOC, ModLib");
        var derivedObject = ScriptableObject.CreateInstance(derivedType);
        var modObject = ScriptableObject.CreateInstance(modType);
        var modPOC = Activator.CreateInstance(modPOCType);
        var gamePOC = new GameLib.GamePOC();
        derivedType.GetField("ModData").SetValue(derivedObject, modPOC);
        modType.GetField("ModData").SetValue(modObject, modPOC);
        derivedType.GetField("GameData").SetValue(derivedObject, gamePOC);
        modType.GetField("GameData").SetValue(modObject, gamePOC);

        //Read field refrences from file and set field refrences here
        gameObject.GetType().GetField("SO").SetValue(gameObject, derivedObject);
        derivedType.GetField("SO").SetValue(derivedObject, gameObject);

        //Add deserialized objects to results
        bundleObjects.Add(derivedObject);
        bundleObjects.Add(modObject);
    }
	// Use this for initialization
	void Start () {
        
        var bundle = AssetBundle.LoadFromFile("../AssetBundles/mod_bundle");
        var results = bundle.LoadAllAssets().ToList();

        var modAssembly = Assembly.LoadFrom("../Libs/ModLib.dll");
        DeserializeModObjectsFromFile(results);

        //Loading finished

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
