namespace TowerDefence
{
    public enum Sound
    {
        BGM = 0,
        Arrow = 1,
        ArrowHit = 2,
        EnemyDie = 3,
        EnemyWin = 4,
        PlayerWin = 5,
        PlayerLose = 6
    }

    public static class SoundExtensions 
    {
        public static void Play(this Sound sound) //Метод расширения enum'а. Стат метод в стат классе
        {
            SoundPlayer.Instance.Play(sound);
        }
    }
}