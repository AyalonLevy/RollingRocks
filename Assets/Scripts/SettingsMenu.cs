using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private float ConvertToDb(float input)
    {
        return Mathf.Log10(input) * 20.0f;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", ConvertToDb(volume));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", ConvertToDb(volume));

        GameData data = GameManager.Instance.GetGameData();
        data.musicVolume = volume;

        SaveSystem.SaveData(data);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", ConvertToDb(volume));

        GameData data = GameManager.Instance.GetGameData();
        data.sfxVolume = volume;

        SaveSystem.SaveData(data);
    }

    public void SetVolumeFromData(float musicVolume, float sfxVolume)
    {
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
    }
}
