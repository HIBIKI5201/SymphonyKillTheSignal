using System;
using UnityEngine;
using static UnityEditor.Progress;

public class UserDataManager : MonoBehaviour
{
    public enum ItemKind
    {
        branch,
        water,
        food,
    }

    public SaveData saveData;
    public void ChangeDistance(int distance)
    {
        saveData.distance += distance;
    }

    public void ChangeTime(int time)
    {
        saveData.time += time;
        saveData.campLevel = Mathf.Max(saveData.campLevel - time, 0);
    }

    public void ChangeHealth(int value)
    {
        saveData.health += value;
        saveData.health = Mathf.Clamp(saveData.health, 0, 100);
    }

    public void ChangeHunger(int value)
    {
        saveData.hunger += value;
        if (saveData.hunger < 0)
        {
            saveData.health -= Mathf.Abs(saveData.hunger) * 2;
        }
        saveData.hunger = Mathf.Clamp(saveData.hunger, 0, 100);
    }

    public void ChangeThirst(int value)
    {
        saveData.thirst += value;
        if (saveData.thirst < 0)
        {
            saveData.health -= Mathf.Abs(saveData.thirst) * 5;
        }
        saveData.thirst = Mathf.Clamp(saveData.thirst, 0, 100);
    }

    public void ChangeBonfireLevel(int value)
    {
        saveData.campLevel = value;
    }

    public void ChangeItemValue(ItemKind kind, int value)
    {
        saveData.itemList[Array.IndexOf(Enum.GetValues(typeof(ItemKind)), kind)] += value;
    }
}
