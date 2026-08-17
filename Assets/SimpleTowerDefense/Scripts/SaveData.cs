using System;

namespace SimpleTowerDefense
{
    /// <summary>
    /// Plain values written to save.json. Keeping this class data-only makes the
    /// save file easy to inspect and explain.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int highScore;
        public int highestWaveReached;
        public int gamesWon;
        public int gamesPlayed;
        public float musicVolume = 0.8f;
        public float sfxVolume = 0.8f;
    }
}
