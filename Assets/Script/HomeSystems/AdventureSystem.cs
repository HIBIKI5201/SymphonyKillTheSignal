using NUnit.Framework;
using System;
using UnityEngine;

namespace AdventureSystems
{
    public class AdventureSystem : MonoBehaviour
    {
        public ItemDataBase itemData;

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

        public static int RestHealHealth(int time, int bonfireLevel)
        {
            int bonfireRestTime = Mathf.Min(bonfireLevel, time);
            //•°‰Î‚ ‚è x^2 + 9x
            int bonfireRestHeal = bonfireRestTime * bonfireRestTime + 9 * bonfireRestTime;
            //•°‰Î‚È‚µ 0.8x^2 - 7,2x
            float normalRestHeal = Mathf.Max(0.8f * time * time + 7.2f * time - (0.8f * bonfireRestTime * bonfireRestTime + 7.2f * bonfireRestTime), 0);
            float result = normalRestHeal + bonfireRestHeal;
            return (int)result;
        }
    }
}
