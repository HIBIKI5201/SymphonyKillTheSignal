using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> itemDataList;

    [Serializable]
    public struct ItemData
    {
        [TextArea]
        public string itemKind;
        public int time;
        public int hunger;

        public int getMinValue;
        public int getMaxValue;
    }
}