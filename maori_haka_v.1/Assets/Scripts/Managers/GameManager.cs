using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentGameState { Playing, Dialogue, DialogueAndLoad, Challenge }

public class GameManager : MonoBehaviour
{
    CurrentGameState gameState;

/*    public static GameManager Instance
    {
        get;
        private set;
    }*/

    private void Awake()
    {
//        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DialogueManager.Instance.OnShowText += () =>
        {
            //when dialogue opened game state dialogue
            //Debug.Log("REGULAR SHOW TEZR");
            gameState = CurrentGameState.Dialogue;
        };

        DialogueManager.Instance.OnShowTextAndStartTest += () =>
        {
            //Debug.Log("SHOW TEXT THEN LOAD TEST");
            gameState = CurrentGameState.DialogueAndLoad;
        };

        DialogueManager.Instance.OnCloseText += () =>
        {
            //when dialogue closed game state is playing
           // gameState = CurrentGameState.Playing;
        };

        TestManager.Instance.OnChallengeOpen += () =>
        {
            gameState = CurrentGameState.Challenge;
        };

        TestManager.Instance.OnChallengeClose += () =>
        {
            gameState = CurrentGameState.Playing;
        };
    }

    private void Update()
    {
        if (gameState == CurrentGameState.DialogueAndLoad)
        {
            DialogueManager.Instance.HandleUpdateAndLoadTest();
            //Debug.Log("DIALOGUE STATE");
        }
        else if (gameState == CurrentGameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
            //Debug.Log("DIALOGUE STATE");
        }
        else if (gameState == CurrentGameState.Challenge)
        {
            TestManager.Instance.HandleUpdate();
            //Debug.Log("CHALLENGE STATE");
        }
        else if(gameState == CurrentGameState.Playing)
        {
            //Debug.Log("PLAYING STATE");
        }
    }
}
