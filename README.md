# ModSerializationDemo

A demo of asset bundle serialization that can support modding.

The there are two components to the project, the [AssetBundle builder 'ModProject'](https://github.com/spacehamster/ModSerializationDemo/tree/master/ModProject) and the [mod .dll builder 'ModLib'](https://github.com/spacehamster/ModSerializationDemo/tree/master/ModLib).
'GameProject' represents a pre-existing unity game.

Game script stubs are extracted from the game's Assembly-CSharp.dll and placed in the [Game Scripts Folder](https://github.com/spacehamster/ModSerializationDemo/tree/master/ModProject/Assets/GameScripts).
Mod script are placed in [Mod Scripts Folder](https://github.com/spacehamster/ModSerializationDemo/tree/master/ModProject/Assets/GameScripts).

This allows unity objects such as `DerivedScriptableObject` to be created through the unity editor and for `GameScriptableObject` and `DerivedScriptableObject` to hold references to each other.
```csharp
public class GameScriptableObject : ScriptableObject
{
    public GamePOC GameData;
    public ScriptableObject SO;
}
public class DerivedScriptableObject : GameScriptableObject
{
    public ModPOC ModData;
}
```

As the unity editor believes ModScripts are part of `Assembly-CSharp.dll`, and because plain [serializable] classes can't be used in asset bundles from ModScripts, they are serialized seperate from the asset bundle.
See [AssetBundleBuilder](https://github.com/spacehamster/ModSerializationDemo/blob/master/ModProject/Assets/Editor/BuildAssetBundles.cs) for the serialization process and [ModLoader](https://github.com/spacehamster/ModSerializationDemo/blob/master/GameProject/Assets/ModScripts/ModLoader.cs) for the deserialization process.
