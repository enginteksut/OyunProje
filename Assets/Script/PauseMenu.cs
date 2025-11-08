using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class PauseMenu : MonoBehaviour
{





    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _howToPlayPanel;
    private bool check;







    private void Start()
    {
        _pauseMenu.SetActive(false);
        _inGameMenu.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        check = false;
    }

    public void ResumeGame()





    {
        _pauseMenu.SetActive(false);
        _inGameMenu.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; // Oyunu devam ettir
        check = false;
    }







    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            check = !check;
            Cursor.visible = check;

            switch (check)
            {
                case true:
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0f; // Oyunu durdur
                    break;
                case false:
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1f; // Oyunu devam ettir
                    break;
            }

            _pauseMenu.SetActive(check);
            _inGameMenu.SetActive(!check);
        }
    }




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
