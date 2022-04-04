using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuHolder;
    [SerializeField] private GameObject settingsHolder;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    [SerializeField] private Image _fadeImage;

    private void Awake()
    {
        Time.timeScale = 1;
        //set cursor to visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _fadeImage.color = Color.black;
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(0, 1f).OnComplete(() => _fadeImage.gameObject.SetActive(false));
    }


    public void StartGame()
    {
        //activate fadeImage and change its alpha from 0 to 1 with DOTween
        //after this LoadScene("Game")
        _fadeImage.color = Color.clear;
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(1, 1).OnComplete(() => SceneManager.LoadScene("Game"));
    }

    public void OpenSettings()
    {
        mainMenuHolder.SetActive(false);
        settingsHolder.SetActive(true);

        //load sliders value from PlayerSettings
        PlayerSettings.Load();
        volumeSlider.value = PlayerSettings.volume;
        sensitivitySlider.value = PlayerSettings.mouseSensitivity;
    }

    public void CloseSettings()
    {
        mainMenuHolder.SetActive(true);
        settingsHolder.SetActive(false);
    }

    public void OnVolumeChanged(float value)
    {
        //Save volume to PlayerSettings
        PlayerSettings.SetVolume(value);
    }

    public void OnSensitivityChanged(float value)
    {
        //Save sensitivity to PlayerSettings
        PlayerSettings.SetMouseSensitivity(value);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        //and show pause menu
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        //and hide pause menu
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}