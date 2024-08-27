using UnityEngine;

namespace AdventureSystems
{
    public class AdventureSystem : MonoBehaviour
    {
        public static int MovementTimeToDistance(int time)
        {
            return time * 3;
        }
        public static int MovementTimeToHealth(int time)
        {
            return time * 5;
        }

        public static int BonfireRequireBranch(int value)
        {
            return value * 5;
        }

        public static int BonfireBecomeLevel(int value)
        {
            return Mathf.Min(value * 3, 8);
        }

        public static int RestHealHealth(int value)
        {
            return value * 5;
        }
    }
}
