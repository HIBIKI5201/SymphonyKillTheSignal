using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveData? _mainSaveData;

    [Serializable]
    public struct SaveData
    {
        public int distance;
        public int time;

        public SaveData(int time, int distance)
        {
            this.distance = distance;
            this.time = time;
        }
    }

    public static void Save()
    {
        Debug.Log(_mainSaveData);
        // インスタンス変数を JSON にシリアル化する
        string json = JsonUtility.ToJson(_mainSaveData);
        // PlayerPrefs に保存する
        PlayerPrefs.SetString("SaveData", json);
    }

    public static SaveData? Load()
    {
        // PlayerPrefs から文字列を取り出す
        string json = PlayerPrefs.GetString("SaveData");
        if (json != null)
        {
            // デシリアライズする
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            //データを返す
            return saveData;
        }
        else
        {
            //セーブデータがない場合にnullを返す
            return null;
        }
    }
}
