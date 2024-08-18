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
