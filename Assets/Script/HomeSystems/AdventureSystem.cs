using UnityEngine;

public class AdventureSystem : MonoBehaviour
{
    public int MovementTimeToDistance(int time)
    {
        return time * 3;
    }
    public int MovementTimeToHealth(int time)
    {
        return time * 5;
    }

    public int BonfireRequireBranch(int value)
    {
        return value * 5;
    }

    public int BonfireBecomeLevel(int value)
    {
        return Mathf.Min(value * 3, 8);
    }
}
