using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftDataBase : ScriptableObject
{
    public List<CraftKind> craftDatas = new();
    [Serializable]
    public struct CraftKind
    {
        public UserDataManager.ItemKind itemKind;
        public int getValue;
        public List<RequireMaterial> requireMaterials;
    }
    [Serializable]
    public struct RequireMaterial
    {
        public UserDataManager.ItemKind materialKind;
        public int value;
    }
}
