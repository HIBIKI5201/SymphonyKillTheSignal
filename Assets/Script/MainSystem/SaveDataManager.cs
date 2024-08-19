using System;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveData? _mainSaveData;

    [Serializable]
    public struct SaveData
    {
        public RealTime saveTime;
        public int distance;
        public int time;

        public SaveData(DateTime dateTime, int time, int distance)
        {
            this.saveTime = new RealTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            this.distance = distance;
            this.time = time;
        }
    }

    [Serializable]
    public struct RealTime
    {
        public int year, month, day, hour, minute, second;
        public RealTime(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
        public override string ToString()
        {
            return $"{year}/{month:D2}/{day:D2} {hour:D2}:{minute:D2}:{second:D2}";
        }
    }

    public static void Save(SaveData saveData)
    {
        //現在のデータを変数に代入
        _mainSaveData = new SaveData(DateTime.Now, saveData.time, saveData.distance);
        //セーブ時刻を確認
        Debug.Log($"{_mainSaveData.Value.saveTime} {_mainSaveData.Value.distance} {_mainSaveData.Value.time}");
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
            //セーブデータを確認
            Debug.Log($"{saveData.saveTime} {saveData.distance} {saveData.time}");
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
