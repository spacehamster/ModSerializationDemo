using GameLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ModLib
{
    [CreateAssetMenu]
    public class ModScriptableObject : ScriptableObject
    {
        public GamePOC GameData;
        public ModPOC ModData;
    }
}