[System.Serializable]
public class GameData
{
    public int maxUnlockedLevel;
    public bool useWASD;
    public bool muteMusic;
    public bool muteEffects;

    public GameData(int maxUnlockedLevel = 1, bool useWASD = false, bool muteMusic = false, bool muteEffects = false)
    {
        this.maxUnlockedLevel = maxUnlockedLevel;
        this.useWASD = useWASD;
        this.muteMusic = muteMusic;
        this.muteEffects = muteEffects;
    }
}
