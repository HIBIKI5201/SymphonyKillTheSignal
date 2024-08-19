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

    public static void Save()
    {
        //���݂̃v���p�e�B���Z�[�u�f�[�^��
        _mainSaveData = new SaveData(DateTime.Now, 5, 5);
        //�Z�[�u�������m�F
        Debug.Log(_mainSaveData.Value.saveTime);
        // �C���X�^���X�ϐ��� JSON �ɃV���A��������
        string json = JsonUtility.ToJson(_mainSaveData);
        // PlayerPrefs �ɕۑ�����
        PlayerPrefs.SetString("SaveData", json);
    }

    public static SaveData? Load()
    {
        // PlayerPrefs ���當��������o��
        string json = PlayerPrefs.GetString("SaveData");
        if (json != null)
        {
            // �f�V���A���C�Y����
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            //�Z�[�u�f�[�^���m�F
            Debug.Log($"{saveData.saveTime.ToString()}\n{saveData.distance}\n{saveData.time}");
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
