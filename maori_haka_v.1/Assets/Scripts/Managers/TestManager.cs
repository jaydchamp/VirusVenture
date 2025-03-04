using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerMoves {Stomp = 1, Slide = 2, Jump = 3 }

public class TestManager : MonoBehaviour
{
    [SerializeField] Dialogue losingChallengeDialogue;
    private UIManager uIManager;

    private bool testPassed = false;
    private bool activeTestFailed = false;
    public bool testStatus = false;
    public bool correctlyAnsweringTest = true;

    private KeyCode currentKeyCode;

    private int numOfDancesInTest;
    private float CorrectAnswers = 0;
    private int CurrentPowerupTest = 1;

    private bool canUseStomp;//1st powerup
    private bool canUseSlide; //2nd pwerup
    private bool canUseJump; //3rd powerup

    public event Action OnChallengeOpen;
    public event Action OnChallengeClose;

    public static TestManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        canUseStomp = false;
        canUseSlide = false;
        canUseJump = false;
        currentKeyCode = KeyCode.None;

        if (uIManager == null)
        {
            //uIManager = GameObject.Find("UIManage").GetComponent<UIManager>();
            uIManager = GetComponent<UIManager>();
        }

        DontDestroyOnLoad(Instance);
    }
    public void InitTest()
    {
        numOfDancesInTest = uIManager.GetTestLength();
        testPassed = false;
        TurnOnChallenge();
        OnChallengeOpen?.Invoke();
    }
    //this is where the test code is run
    public void HandleUpdate()
    {
        //Debug.Log("CurrentPowerupTest: " + CurrentPowerupTest);

        //Debug.Log(activeTestFailed);
        if(uIManager == null)
        {
            uIManager = GameObject.FindGameObjectWithTag("UIManage").GetComponent<UIManager>();
        }

        if (testStatus)
        {
            currentKeyCode = uIManager.GetCurrentDanceDirection();
            //Debug.Log(currentKeyCode);

            if (correctlyAnsweringTest && Input.GetKeyDown(currentKeyCode))
            {
                //answered the arrow correctly, onto next one
                correctlyAnsweringTest = true;
                CorrectAnswers++;

                //while the number of correct inputs is less than the total amount
                if (CorrectAnswers <= numOfDancesInTest)
                {
                    //move to next input
                    activeTestFailed = false;
                    uIManager.NextDanceIndex();
                }
            }
            else if (correctlyAnsweringTest && CorrectAnswers == numOfDancesInTest)
            {
                //USER HAS SUCCESSFULLY PASSED THE CHALLENGE
                activeTestFailed = false;
                GameWon();
            }
            else if (Input.anyKeyDown)
            {
                //Debug.Log("Test FAILED");
                uIManager.DeactivateTest();
                TurnOffChallenge();
                activeTestFailed = true;

                StartCoroutine(DialogueManager.Instance.ShowDialogue(losingChallengeDialogue));

                DialogueManager.Instance.OnCloseText += () =>
                {
                    //only restart test when the current test they are on is failed
                    if(activeTestFailed)
                    {
                        //Debug.Log("TEST MANAGER DIALOGUE CLOSED TEST TURNED ON");
                        correctlyAnsweringTest = true;
                        CorrectAnswers = 0;
                        activeTestFailed = false;
                        uIManager.ResetTest();
                        TurnOnChallenge();
                    }
                };
            }
        }
    }

    public bool GetTestPassValue()
    {
        return testPassed;
    }
    public void SetTestPassValue(bool val)
    {
        testPassed = val;
    }
    public void SetTestStatus(bool set)
    {
        testStatus = set;
    }

    public bool GetTestStatus()
    {
        return testStatus;
    }

    public void TurnOnChallenge()
    {
        uIManager.ActivateTest();
        //Debug.Log("SETTING CHALLENGE TRUE");
        uIManager.SetChallengeStatus(true);
        testStatus = true;
        OnChallengeOpen?.Invoke();
    }

    public void TurnOffChallenge()
    {
        CorrectAnswers = 0;
        correctlyAnsweringTest = true;
        testStatus = false;
        activeTestFailed = false;

        //uIManager.currentTestArrowIndex = 0;
        //Debug.Log("SETTING CHALLENGE FALSE");
        uIManager.SetChallengeStatus(false);
        uIManager.ResetTest();
        uIManager.DeactivateAllUUI();
        OnChallengeClose?.Invoke();
    }

    public void GameWon()
    {
        //hakaMemberFollow.SetFollowStatus(true);
        testPassed = true;
        SetPowerupStatus(CurrentPowerupTest, true);
        CurrentPowerupTest++;
        uIManager.NextTestSequence(CurrentPowerupTest);

        //JORGE MOD BELOW
        GameObject emmaWave = GameObject.Find("AntiVirusWave");
        GameObject levelLock = GameObject.Find("Lock");
        emmaWave.gameObject.GetComponent<SuperWave>().spawnWave();
        levelLock.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        levelLock.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //levelLock.gameObject.SetActive(false);
        //JORGE MOD ABOVE

        uIManager.InitTest();
        OnChallengeClose?.Invoke();
        TurnOffChallenge();
        //CustomSceneManager.Instance.MoveToNextScene(); 
    }

    public void SetPowerupStatus(int powerupNum, bool powerupStatus)
    {
        if (powerupNum == 0) //tutorial
        {
            Debug.Log("Tutorial Complete");
        }
        else if (powerupNum == 1) //stomp
        {
            Debug.Log("STOMP GAINED " + powerupStatus);
            canUseStomp = powerupStatus;
        }
        else if (powerupNum == 2) //slide
        {
            Debug.Log("SLIDE GAINED " + powerupStatus);
            canUseSlide = powerupStatus;
        }
        else if (powerupNum == 3) //jump
        {
            Debug.Log("JUMP GAINED " + powerupStatus);
            canUseJump = powerupStatus;
        }
        else
        {
            //Debug.Log("Invalid Powerup Number");
        }
    }

    public bool GetPowerupStatus(int powerupNum)
    {
        if (powerupNum == 1)//stomp 
        {
            return canUseStomp;
        }
        else if (powerupNum == 2) //slide
        {
            return canUseSlide;
        }
        else if (powerupNum == 3) //jump
        {
            return canUseJump;
        }
        else
        {
            Debug.Log("Invalid Powerup Num");
            return false;
        }
    }

    //returns 1 for stomp scene, 2 for slide scene, and 3 for jump scene
    public int GetTestNumber()
    {
        return CurrentPowerupTest;
    }

    public float GetNumCorrectAnswers()
    {
        return CorrectAnswers;
    }
}
