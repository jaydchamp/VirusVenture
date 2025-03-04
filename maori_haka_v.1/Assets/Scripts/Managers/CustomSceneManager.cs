using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    GameManagerCreatorScene = 0,
    MainMenu = 1,
    Credits = 2,
    IntroCutscene = 3,
    TutorialLevel1 = 4,
    Level2 = 5,
    Level3 = 6,
    Level4 = 7,
    GameOver = 8
}

public class CustomSceneManager : MonoBehaviour
{
    private int currentScene;
    public UIManager UIManager;

    public static CustomSceneManager Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        Instance = this;
        currentScene = (int)Scene.MainMenu;
        SceneManager.LoadScene(currentScene);
        DontDestroyOnLoad(gameObject);
    }

/*    private void Update()
    {
        Debug.Log("CURRENT SCENE: " + currentScene);
    }*/
    public int GetCurrentSceneInt()
    {
        return currentScene;
    }
    public void MoveToNextScene()
    {
        currentScene++;
        Debug.Log("MOVING TO SCENE NUMBER: " + currentScene);

        if(currentScene == 8)
        {
            Debug.Log("MOVING TO FINAL SCENE");
            UIManager.DeactivateAllUUI();
            UIManager.ActivateEnding();
            SceneManager.LoadScene(currentScene);
        }
        else
        {
            SceneManager.LoadScene(currentScene);
        }
    }

    public void MoveToSceneInt(int scene)
    {
        currentScene = scene;
        Debug.Log("MOVING TO SCENE NUMBER: " + currentScene);

        if(currentScene == 8)
        {
            Debug.Log("MOVING TO FINAL SCENE");
            UIManager.DeactivateAllUUI();
            UIManager.ActivateEnding();
            SceneManager.LoadScene(scene);
        }
        else 
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void PlayGame()
    {
        currentScene = (int)Scene.IntroCutscene;
        UIManager.DeactivateMainMenu();
        UIManager.DeactivateEnding();
        MoveToSceneInt((int)Scene.IntroCutscene);
    }

    public void MenuLoad()
    {
        currentScene = (int)Scene.MainMenu;
        UIManager.DeactivateCredits();
        UIManager.DeactivateEnding();
        UIManager.ActivateMainMenu();
        MoveToSceneInt((int)Scene.MainMenu);
    }
    public void CreditsScene()
    {
        currentScene = (int)Scene.Credits;
        UIManager.DeactivateMainMenu();
        UIManager.ActivateCredits();
        MoveToSceneInt((int)Scene.Credits);
    }

    public void GameOverScene()
    {
        UIManager.DeactivateAllUUI();
        currentScene = (int)Scene.GameOver;
        MoveToSceneInt((int)Scene.GameOver);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
