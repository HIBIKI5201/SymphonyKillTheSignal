using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public SaveData saveData;

    public void ChangeDistance(int distance)
    {
        saveData.distance += distance;
    }

    public void ChangeTime(int time)
    {
        saveData.time += time;
    }

    public void ChangeHealth(int value)
    {
        saveData.health += value;
        saveData.health = Mathf.Clamp(saveData.health, 0, 100);
    }

    public void ChangeHunger(int value)
    {
        saveData.hunger += value;
        saveData.hunger = Mathf.Clamp(saveData.hunger, 0, 100);
    }

    public void ChangeThirst(int value)
    {
        saveData.thirst += value;
        saveData.thirst = Mathf.Clamp(saveData.thirst, 0, 100);
    }

}
