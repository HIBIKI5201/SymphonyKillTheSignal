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
        //���݂̃f�[�^��ϐ��ɑ��
        _mainSaveData = saveData;
        // �C���X�^���X�ϐ��� JSON �ɃV���A��������
        string json = JsonUtility.ToJson(_mainSaveData);
        Debug.Log(json);
        // PlayerPrefs �ɕۑ�����
        PlayerPrefs.SetString("SaveData", json);
    }

    public static SaveData Load()
    {
        // PlayerPrefs ���當��������o��
        string json = PlayerPrefs.GetString("SaveData");
        Debug.Log(json);
        // �f�V���A���C�Y����
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        if (json != null)
        {
            //�f�[�^��Ԃ�
            return saveData;
        }
        else
        {
            //�Z�[�u�f�[�^���Ȃ��ꍇ��null��Ԃ�
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