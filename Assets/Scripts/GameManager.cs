using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This script controls everything related to current scene
// Manage of game's cutscenes
// This class is a singleton
public class GameManager : MonoBehaviour
{
    [SerializeField] public CharacterController characterController;
    [SerializeField] public BlackHoleController blackHoleController;
    [SerializeField] public UIManager uiManager;
    [SerializeField] public Image _whiteFade;

    [SerializeField] private Converter _converter;

    [SerializeField] public Material antiMatterMaterial;
    [SerializeField] public ParticleSystem antiMatterParticleSystem;

    [SerializeField] private GameObject _anihilationEnding;
    [SerializeField] private GameObject _fallIntoBlackHoleEnding;
    [SerializeField] private GameObject _goodEnding;
    [SerializeField] private GameObject _badEnding;

    // Singleton
    public static GameManager Instance;

    public bool IsCutscenePlaying = false;

    private bool isGameOver = false;


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

        PlayCutscene();
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
        if (IsCutscenePlaying) return;
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

    public void GameOverFallIntoBlackHole()
    {
        isGameOver = true;
        //Disable player movement and interaction
        characterController.SetMovement(false);
        characterController.SetInteraction(false);

        //Show _fallIntoBlackHoleEnding screen
        _fallIntoBlackHoleEnding.SetActive(true);
    }

    public async void PlayCutscene()
    {
        IsCutscenePlaying = true;
        characterController.SetMovement(false);
        characterController.SetInteraction(false);

        //Play cutscene
        await UniTask.Delay(200);

        List<string> cutscene = new List<string>();
        cutscene.Add("FINALY! I DID IT! The work of my life is complete!");
        cutscene.Add("I have created a miniature black hole!");
        cutscene.Add("Well, what am I going to do with it?");
        cutscene.Add("Wait a minute... Is it growing?");
        cutscene.Add("SHIT! I'm going to die!");


        await uiManager.ShowMessages(cutscene);

        await UniTask.Delay(200);

        //End cutscene
        characterController.SetMovement(true, true);
        characterController.SetInteraction(true);
        IsCutscenePlaying = false;
    }

    public async void PlayConverterCutscene()
    {
        IsCutscenePlaying = true;
        characterController.SetMovement(false);
        characterController.SetInteraction(false);

        //Play cutscene
        await UniTask.Delay(200);

        List<string> cutscene = new List<string>();
        cutscene.Add("Oh yes! This is my anti-matter converter!");
        cutscene.Add("Black hole grows because it consumes matter around it");
        cutscene.Add("And with help of this device I can convert matter into ANTI-MATTER!");
        cutscene.Add("I hope this will help me shrink the black hole...");
        cutscene.Add("OK. I'm going to try it!");


        await uiManager.ShowMessages(cutscene);

        await UniTask.Delay(200);

        //End cutscene
        characterController.SetMovement(true, true);
        characterController.SetInteraction(true);


        uiManager.ShowAntiMatterTutorial();

        IsCutscenePlaying = false;
    }

    public async void PlayAntiMatterConvertedCutscene()
    {
        IsCutscenePlaying = true;
        characterController.SetMovement(false);
        characterController.SetInteraction(false);

        //Play cutscene
        await UniTask.Delay(200);

        List<string> cutscene = new List<string>();

        cutscene.Add("Nice! Now this object made of anti-matter. Let's try to throw it into black hole.");


        await uiManager.ShowMessages(cutscene);

        await UniTask.Delay(200);

        //End cutscene
        characterController.SetMovement(true, true);
        characterController.SetInteraction(true);
        IsCutscenePlaying = false;
    }

    public async void PlayAntiMatterConsumeCutscene()
    {
        IsCutscenePlaying = true;
        characterController.SetMovement(false);
        characterController.SetInteraction(false);

        //Play cutscene
        await UniTask.Delay(200);

        List<string> cutscene = new List<string>();
        cutscene.Add("YES! This it's working!");
        cutscene.Add(
            "All I need to do now is to convert more objects into anti-matter and throw them into the black hole!");


        await uiManager.ShowMessages(cutscene);

        await UniTask.Delay(200);

        //End cutscene
        characterController.SetMovement(true, true);
        characterController.SetInteraction(true);
        IsCutscenePlaying = false;
    }

    public async void PlayEndingCutscene()
    {
        isGameOver = true;
        IsCutscenePlaying = true;
        characterController.SetMovement(false);
        characterController.SetInteraction(false);


        characterController.LookAt(blackHoleController.transform, true, false);

        //Play cutscene
        await UniTask.Delay(200);

        List<string> cutscene = new List<string>();
        cutscene.Add("Well, I don't have anything else to throw into black hole...");
        cutscene.Add("But maybe...");

        //Point gun at camera


        await uiManager.ShowMessages(cutscene);


        characterController.SetMovement(false);
        characterController.SetInteraction(false);

        await UniTask.Delay(1000);

        cutscene.Clear();

        cutscene.Add("I'll do this for science!");

        await uiManager.ShowMessages(cutscene);


        ShowWhiteFade();

        await UniTask.Delay(2000);

        //End cutscene

        if (blackHoleController.currentSize < 1)
        {
            _goodEnding.SetActive(true);
        }
        else
        {
            _badEnding.SetActive(true);
        }
    }

    public void ShowWhiteFade()
    {
        _whiteFade.DOFade(1, 0.5f);
    }
}