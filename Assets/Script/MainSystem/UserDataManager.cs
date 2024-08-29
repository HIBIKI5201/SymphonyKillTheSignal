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
}
