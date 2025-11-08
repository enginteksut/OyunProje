using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class Interface : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _howToPlayPanel;

    private void Start()
    {
        _settings.SetActive(false);
    }

    // Oyunu Başlat → Trailer sahnesine yönlendir
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Trailer");
    }

    // Ayarlar panelini aç/kapat
    public void SettingsOpen()
    {
        _settings.SetActive(true);
    }

    public void SettingsClose()
    {
        _settings.SetActive(false);
    }

    // Grafik Ayarları
    public void VeryLow() => QualitySettings.SetQualityLevel(0);
    public void Low() => QualitySettings.SetQualityLevel(1);
    public void Medium() => QualitySettings.SetQualityLevel(2);
    public void High() => QualitySettings.SetQualityLevel(3);
    public void VeryHigh() => QualitySettings.SetQualityLevel(4);
    public void Ultra() => QualitySettings.SetQualityLevel(5);

    // Oyundan çıkış
    public void QuitGame()
    {
        Application.Quit();
    }

    // Nasıl oynanır? paneli
    public void ShowHowToPlay()
    {
        _howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        _howToPlayPanel.SetActive(false);
    }

    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
