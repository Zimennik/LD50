using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls everything related to current scene
// Manage of game's cutscenes
// This class is a singleton
public class GameManager : MonoBehaviour
{
    [SerializeField] public CharacterController characterController;
    [SerializeField] public BlackHoleController blackHoleController;
    [SerializeField] public UIManager uiManager;

    [SerializeField] public Material antiMatterMaterial;
    [SerializeField] public ParticleSystem antiMatterParticleSystem;

    [SerializeField] private GameObject _anihilationEnding;

    // Singleton
    public static GameManager Instance;


    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
        //TEST
        //EndCutscene();


        //set cursor to not be visible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    //Load settings from PlayerSettings script and apply them
    public void LoadSettings()
    {
        //Load settings from PlayerSettings script
        PlayerSettings.Load();
        //Apply mouse sensitivity
        characterController.SetMouseSensitivity(PlayerSettings.mouseSensitivity);
        //Set the volume of the game
        AudioListener.volume = PlayerSettings.volume;
    }

    //Methode that called when game starts
    //Player interaction and movement is disabled until start cutscene is over
    public void StartGame()
    {
        characterController.SetMovement(false);
        characterController.SetInteraction(false);
    }

    //Methode that called when cutscene is over
    //Player interaction and movement is enabled
    public void EndCutscene()
    {
        characterController.SetMovement(true);
        characterController.SetInteraction(true);
    }

    //Methode that called when player dies
    //Player interaction and movement is disabled
    public void PlayerDied()
    {
        characterController.SetMovement(false);
        characterController.SetInteraction(false);
    }

    public void AnihilationEnding()
    {
        characterController.SetMovement(false);
        characterController.SetInteraction(false);
        _anihilationEnding.SetActive(true);
    }

    public void PauseGame()
    {
        if (Time.timeScale > 0)
        {
            uiManager.Pause();
        }
        else
        {
            uiManager.Unpause();
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}