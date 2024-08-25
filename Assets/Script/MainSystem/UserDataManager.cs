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
}