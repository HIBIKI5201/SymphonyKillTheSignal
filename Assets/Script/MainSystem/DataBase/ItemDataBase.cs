using System;
using System.Collections.Generic;
using UnityEngine;
using static UserDataManager;
[CreateAssetMenu]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> itemDataList;

    [Serializable]
    public struct ItemData
    {
        public ItemKind kind;
        public ItemCollectData collectData;
        public ItemInventoryData inventoryData;
    }

    [Serializable]
    public struct ItemInventoryData
    {
        [TextArea]
        public string explanation;
        public List<ItemEfficacy> itemEfficacy;
    }
    [Serializable]
    public struct ItemEfficacy
    {
        public StatusKind statusKind;
        public int value;
    }

    [Serializable]
    public struct ItemCollectData
    {
        [TextArea]
        public string itemName;
        public int time;
        public int hunger;

        public int getMinValue;
        public int getMaxValue;
    }
}