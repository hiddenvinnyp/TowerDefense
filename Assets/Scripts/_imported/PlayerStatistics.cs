using UnityEngine;

namespace SpaceShooter
{
    public class PlayerStatistics
    {
        public int NumKills;
        public int Score;
        public float Time;
        public float TimeBonus;

        public void ResetStats()
        {
            NumKills = 0;
            Score = 0;
            Time = 0;
            TimeBonus = 0;
        }
    }
}
