[System.Serializable]
public class GameData
{
    public int maxUnlockedLevel;
    public bool useWASD;
    public float musicVolume;
    public float sfxVolume;

    public GameData(int maxUnlockedLevel = 1, bool useWASD = false, float musicVolume = 0.5f, float sfxVolume = 0.8f)
    {
        this.maxUnlockedLevel = maxUnlockedLevel;
        this.useWASD = useWASD;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
    }
}
