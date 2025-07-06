using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider sfxSlider;

    void Start()
    {
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxSlider.value = sfxVol;

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in audioSources) {
            source.volume = sfxVol;
        }

        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in audioSources) {
            source.volume = volume;
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (settingsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }
}