using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int maxUnlockedLevel;
    public bool useWASD;
    public float musicVolume;
    public float sfxVolume;
    public List<CollectableSO> playerCollectabels;

    public GameData(int maxUnlockedLevel = 1, bool useWASD = false, float musicVolume = 0.5f, float sfxVolume = 0.8f, List<CollectableSO> playerCollection = null)
    {
        this.maxUnlockedLevel = maxUnlockedLevel;
        this.useWASD = useWASD;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;

        if (playerCollection == null)
        {
            this.playerCollectabels = new List<CollectableSO>();
        }
        else
        {
            this.playerCollectabels = playerCollection;
        }
    }
}
