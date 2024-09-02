using System;
using UnityEngine;
using static SaveDataManager;

public class SaveDataManager : MonoBehaviour
{
    public static SaveData _mainSaveData;

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
    public RealTime saveTime;
    public int distance;
    public int time;
    public int health;
    public int hunger;
    public int thirst;
    public int campLevel;

    public ItemList itemList;

    public SaveData(DateTime dateTime, int time, int distance, int health, int hunger, int thirst, int campLevel, ItemList itemList)
    {
        this.saveTime = new RealTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        this.distance = distance;
        this.time = time;
        this.health = health;
        this.hunger = hunger;
        this.thirst = thirst;
        this.campLevel = campLevel;
        this.itemList = itemList;
    }
}
[Serializable]
public struct ItemList
{
    public int branch;

    public ItemList(int branch)
    {
        this.branch = branch;
    }
}