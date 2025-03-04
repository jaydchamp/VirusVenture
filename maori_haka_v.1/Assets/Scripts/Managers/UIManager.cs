using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class DanceOff
    {
        public GameObject danceObj;
        public Image danceImage;
        public KeyCode danceDirection;
        public Sprite onSprite;
        public Sprite offSprite;
        public Transform correspondingPopUp;
    }

    public bool insideChallenge;

    private Transform tempTestPoint;
    private DanceOff[] challengeInfo;

    private Transform tempPopUp;
    private Transform[] popUpInfo;
    private int length;

    private Transform mainMenuObjects;
    private Transform creditsObjects;
    private Transform endingSceneObjects;

    public int currentTestArrowIndex;

    private GameObject currentDanceObj;
    private Image currentDanceImage;
    private KeyCode currentTestDirection;

    public Sprite OnLeftArrow;
    public Sprite OnRightArrow;
    public Sprite OnDownArrow;
    public Sprite OnUpArrow;
    public Sprite OffLeftArrow;
    public Sprite OffRightArrow;
    public Sprite OffDownArrow;
    public Sprite OffUpArrow;

    private Transform progressionBar;
    private Image correctBar;
    //private Image wrongBar;

    void Start()
    {
        tempTestPoint = transform.Find("Tutorial");
        tempPopUp = tempTestPoint.Find("TutorialPopUp");
        progressionBar = transform.Find("ProgBar");

        correctBar = progressionBar.GetChild(1).GetComponent<Image>();
        correctBar.fillAmount = Mathf.Clamp(correctBar.fillAmount, 0.0f, 1.0f);
        correctBar.fillAmount = 0.0f;

        mainMenuObjects = transform.Find("MainMenu");
        creditsObjects = transform.Find("Credits");
        endingSceneObjects = transform.Find("Ending");
        if (endingSceneObjects == null) { Debug.Log("NO ENDING SCENE OBJECTS"); };

        currentTestArrowIndex = 0;

        InitTest();
        DeactivateAllUUI();
    }

    void Update()
    {
        if (insideChallenge)
        {
            currentDanceObj = challengeInfo[currentTestArrowIndex].danceObj;
            currentDanceImage = challengeInfo[currentTestArrowIndex].danceImage;
            currentTestDirection = challengeInfo[currentTestArrowIndex].danceDirection;
        }
        else
        {
            currentDanceObj = null;
            currentDanceImage = null;
            currentTestDirection = KeyCode.None;
        }
    }

    public void InitTest()
    {
        length = 5;//challengeInfo.Length;
        challengeInfo = new DanceOff[length];
        popUpInfo = new Transform[length];
       // popUpInfo = new Image[length];

        //Debug.Log(length);

        if(length <= 0 || length == null)
        {
            Debug.Log("NO ITEMS" + length);
            return;
        }

        for (int i = 0; i < length; i++)
        {
            Transform sequenceTransform = tempTestPoint.Find("sequence" + i);
            //Debug.Log(tempPopUp);
            Transform test = tempPopUp.Find("popup" + i);

            popUpInfo[i] = test;

            //none left break loop
            if(sequenceTransform == null)
            {
                Debug.Log("None Left loop Broken");
                break;
            }

            string tag = sequenceTransform.tag;
            //Debug.Log(sequenceTransform);

            Sprite tempOnSprite;
            Sprite tempOffSprite;
            KeyCode temp;

            switch (tag)
            {
                case "left":
                    tempOnSprite = OnLeftArrow;
                    tempOffSprite = OffLeftArrow;
                    temp = KeyCode.LeftArrow;
                    break;

                case "right":
                    tempOnSprite = OnRightArrow;
                    tempOffSprite = OffRightArrow;
                    temp = KeyCode.RightArrow;
                    break;

                case "up":
                    tempOnSprite = OnUpArrow;
                    tempOffSprite = OffUpArrow;
                    temp = KeyCode.UpArrow;
                    break;

                case "down":
                    tempOnSprite = OnDownArrow;
                    tempOffSprite = OffDownArrow;
                    temp = KeyCode.DownArrow;
                    break;

                default:

                    Debug.Log("Invalid KeyCode Tag");
                    continue; // Skip to the next iteration
            }

            challengeInfo[i] = new DanceOff
            {
                danceObj = tempTestPoint.Find("sequence" + (i)).gameObject,
                danceImage = tempTestPoint.Find("sequence" + (i)).GetComponent<Image>(),
                danceDirection = temp,
                onSprite = tempOnSprite,
                offSprite = tempOffSprite,
                correspondingPopUp = popUpInfo[i],
            };
        } 
    }

    public void DeactivateTest()
    {
        //Debug.Log("Test DEACTIVATION");
        tempTestPoint.gameObject.SetActive(false);
        progressionBar.gameObject.SetActive(false);
    }

    public void ActivateTest()
    {
        //Debug.Log("ACTIVATION");
        tempTestPoint.gameObject.SetActive(true);
        progressionBar.gameObject.SetActive(true);
        tempPopUp.gameObject.SetActive(true);
    }

    public void DeactivateAllUUI()
    {
        //Debug.Log("TURN EVERYTHING OFF");
        currentTestArrowIndex = 0;

        foreach (Transform child in transform)
        {
            //if it's the task bar, leave it alone
            if (child.gameObject.name == "TaskBar" || 
                child.gameObject.name == "MainMenu") 
            { continue; }

            child.gameObject.SetActive(false);
        }
    }

    public void ActivateAllUUI()
    {
        //Debug.Log("TURN IT ALL ON");
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void SetChallengeStatus(bool set)
    {
        insideChallenge = set;
    }

    public void NextDanceIndex()
    {
        //darken based on currentTestIndex then move on
        if (currentDanceImage != null)
        {
            currentDanceImage.sprite = challengeInfo[currentTestArrowIndex].offSprite;
            popUpInfo[currentTestArrowIndex].gameObject.SetActive(false);
            currentTestArrowIndex++;
        }
        else
        {
            Debug.Log("Image was not Found?");
        }

        IncrementProgBar();
    }

    public int GetCurrentDanceIndex()
    {
        return currentTestArrowIndex;
    }

    public void ResetTest()
    {
        //Debug.Log("Test was reset");
        currentTestArrowIndex = 0;
        Image tempImage;

        for (int i = 0; i < length; i++)
        {
            tempImage = challengeInfo[i].danceImage;
            tempImage.sprite = challengeInfo[i].onSprite;
            popUpInfo[i].gameObject.SetActive(true);

            if(tempImage == null)
            {
                break;
            }
        }

        ResetProgBar();
    }

    public KeyCode GetCurrentDanceDirection()
    {
        return currentTestDirection;
    }

    public int GetTestLength()
    {
        return length;
    }

    public void NextTestSequence(int num)
    {
        //change which sequence will be init upon running initTest()
        if (num == 1)
        {
            tempTestPoint = transform.Find("TestOneStomp");
            tempPopUp = tempTestPoint.Find("TestOnePopUp");
        }
        else if (num == 2)
        {
            tempTestPoint = transform.Find("TestTwoSlide");
            tempPopUp = tempTestPoint.Find("TestTwoPopUp");
        }
        else if (num == 3)
        {
            tempTestPoint = transform.Find("TestThreeJump");
            tempPopUp = tempTestPoint.Find("TestThreePopUp");
        }
        else
        {
            Debug.Log("Invalid Test Number: " + num);
        }
    }

    public void IncrementProgBar()
    {
        if (correctBar != null)
        {
            correctBar.fillAmount = TestManager.Instance.GetNumCorrectAnswers() / 5.0f;//correctBar.fillAmount + (1 / 6);//TestManager.Instance.GetNumCorrectAnswers() / 5;
        }
        else
        {
            Debug.LogError("ProgressionBar not found!");
        }
    }

    public void ResetProgBar()
    {
        if (correctBar != null)
        {
            //Debug.Log("REEEEEEEEESEEETTTTTT");
            correctBar.fillAmount = 0f;
        }
        else
        {
            Debug.LogError("ProgressionBar not found!");
        }
    }

    public void DeactivateMainMenu()
    {
        mainMenuObjects.gameObject.SetActive(false);
    }

    public void DeactivateCredits()
    {
        creditsObjects.gameObject.SetActive(false);
    }

    public void DeactivateEnding()
    {
        endingSceneObjects.gameObject.SetActive(false);
    }

    public void ActivateMainMenu()
    {
        mainMenuObjects.gameObject.SetActive(true);
    }

    public void ActivateCredits()
    {
        creditsObjects.gameObject.SetActive(true);
    }

    public void ActivateEnding()
    {
        endingSceneObjects.gameObject.SetActive(true);
    }
}
