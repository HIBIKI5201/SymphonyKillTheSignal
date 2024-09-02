using UnityEngine;

namespace AdventureSystems
{
    public class AdventureSystem : MonoBehaviour
    {
        public static int MovementTimeToDistance(int time)
        {
            float result = 4 * time * (1 + 0.1f * time - 0.1f);
            return (int)result;
        }
        public static int MovementTimeToHunger(int time)
        {
            return time * 9;
        }
        public static int MovementTimeToHealth(int time)
        {
            return time * 3;
        }

        public static int BonfireRequireBranch(int value)
        {
            return value * 5;
        }

        public static int BonfireBecomeLevel(int value)
        {
            return Mathf.Min(value * 3, 8);
        }

        public static int BonfireBranch(int value)
        {
            return value * 5;
        }

        public static int RestHealHealth(int value)
        {
            return value * 5;
        }
    }
}
