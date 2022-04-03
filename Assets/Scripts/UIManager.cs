using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public CrosshairController crosshairController;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public Slider _sensetivitySlider;
    [SerializeField] public Slider _volumeSlider;


    //Load senseetivity and volume from PlayerSettings with LoadMehtod and apply them to the sliders
    public void Pause()
    {
        GameManager.Instance.characterController.SetMovement(false);
        GameManager.Instance.characterController.SetInteraction(false);

        //timescale is set to 0 to stop the game
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        PlayerSettings.Load();

        //show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        crosshairController.SetCroshairVisibility(false);

        _sensetivitySlider.value = PlayerSettings.mouseSensitivity;
        _volumeSlider.value = PlayerSettings.volume;
    }

    public void Unpause()
    {
        //time scale is set to 1 to resume the game
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        //move cursor to center of screen

        GameManager.Instance.characterController.SetMovement(true);
        GameManager.Instance.characterController.SetInteraction(true);

        //hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        crosshairController.SetCroshairVisibility(true);
    }

    public void OnSensetivitySliderChange(float value)
    {
        PlayerSettings.SetMouseSensitivity(value);
        GameManager.Instance.LoadSettings();
    }

    public void OnVolumeSliderChange(float value)
    {
        PlayerSettings.SetVolume(value);
    }
}