using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SaveDataManager;
using static UserDataManager;

public class SaveDataManager : MonoBehaviour
{
    public static SaveData _mainSaveData;

    [Serializable]
    public struct RealTime
    {
        public List<int> time;
        public RealTime(DateTime date)
        {
            time = Enumerable.Repeat(0, 6).ToList();
            time[0] = date.Year;
            this.time[1] = date.Month;
            this.time[2] = date.Day;
            this.time[3] = date.Hour;
            this.time[4] = date.Minute;
            this.time[5] = date.Second;
        }
        public override readonly string ToString()
        {
            return $"{time[0]}/{time[1]:D2}/{time[2]:D2} {time[3]:D2}:{time[4]:D2}:{time[5]:D2}";
        }
    }

    public static void Save(SaveData saveData)
    {
        //現在のデータを変数に代入
        _mainSaveData = saveData;
        // インスタンス変数を JSON にシリアル化する
        string json = JsonUtility.ToJson(_mainSaveData);
        Debug.Log(json);
        // PlayerPrefs に保存する
        PlayerPrefs.SetString("SaveData", json);
    }

    public static SaveData Load()
    {
        // PlayerPrefs から文字列を取り出す
        string json = PlayerPrefs.GetString("SaveData");
        Debug.Log(json);
        // デシリアライズする
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        if (json != null)
        {
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

[Serializable]
public class SaveData
{
    public List<int> saveDate;
    public int distance;
    public int time;
    public int health;
    public int hunger;
    public int thirst;
    public int campLevel;
    public WorldManager.Weather weather;
    public List<bool> storyProgress;

    public List<int> itemList;
    public SaveData(int time, int distance, int health, int hunger, int thirst, int campLevel, WorldManager.Weather weather, List<bool> storyTextDatas)
    {
        saveDate = new RealTime(DateTime.Now).time;
        this.distance = distance;
        this.time = time;
        this.health = health;
        this.hunger = hunger;
        this.thirst = thirst;
        this.campLevel = campLevel;
        this.itemList = new List<int>(new int[Enum.GetValues(typeof(ItemKind)).Length]);
        this.weather = weather;
        storyProgress = storyTextDatas;
    }
}