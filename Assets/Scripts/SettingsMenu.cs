using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public AudioMixer audioMixer;
    public Slider sfxSlider;

    void Start()
    {
        float sfxVol;
        audioMixer.GetFloat("SFXVolume", out sfxVol);
        sfxSlider.value = Mathf.Pow(10, sfxVol / 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
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