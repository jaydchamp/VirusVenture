using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IntroScript : MonoBehaviour
{
    public GameObject hugoSprite;
    public GameObject maxSprite;
    public GameObject mmSprite;
    public GameObject compSprite;
    public GameObject bg;
    private float shot; 
    public GameObject[] boxes;
    [SerializeField] GameObject whiteBox;
    public bool waitingForFade;

    IEnumerator WaitForFade()
    {
        waitingForFade = true;
        yield return new WaitForSeconds(4.0f);
        boxes[10].SetActive(false);
        boxes[11].SetActive(true);
        bg.SetActive(false);
        hugoSprite.SetActive(false);
        maxSprite.SetActive(false);
        mmSprite.SetActive(false);
        compSprite.SetActive(false);
        whiteBox.GetComponent<WhiteFade>().FadeIn();
        waitingForFade = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        waitingForFade = false;
        shot = 0;
        whiteBox.GetComponent<WhiteFade>().FadeIn();
        whiteBox.GetComponent<WhiteFade>().Still();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitingForFade)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                shot++;
                CutsceneManagement();
            }
        }
    }

    void CutsceneManagement()
    {
        if (shot == 0)
        {
            bg.SetActive(true);
            compSprite.SetActive(true);
            mmSprite.SetActive(true);
            boxes[0].SetActive(true);  
        }
        else if (shot == 1) 
        {
            boxes[1].SetActive(true);
        }
        else if (shot == 2)
        {
            boxes[0].SetActive(false);
            boxes[1].SetActive(false);
            maxSprite.SetActive(true);
            boxes[2].SetActive(true);
        }
        else if (shot == 3)
        {
            boxes[3].SetActive(true);
            boxes[4].SetActive(true);
        }
        else if (shot == 4)
        {
            boxes[2].SetActive(false);
            boxes[3].SetActive(false);
            boxes[4].SetActive(false);
            boxes[5].SetActive(true);
        }
        else if (shot == 5)
        {
            boxes[5].SetActive(false);
            hugoSprite.SetActive(true);
            boxes[6].SetActive(true);
        }
        else if (shot == 6)
        {
            boxes[6].SetActive(false);
            boxes[7].SetActive(true);
        }
        else if (shot == 7)
        {
            boxes[7].SetActive(false);
            boxes[8].SetActive(true);
        }
        else if (shot == 8)
        {
            boxes[8].SetActive(false);
            boxes[9].SetActive(true);
        }
        else if (shot == 9)
        {
            boxes[9].SetActive(false);
            boxes[10].SetActive(true);
        }
        else if (shot == 10)
        {
            whiteBox.GetComponent<WhiteFade>().FadeOut();
            StartCoroutine("WaitForFade");
            //bg.SetActive(false);
            //hugoSprite.SetActive(false);
            //maxSprite.SetActive(false);
            //mmSprite.SetActive(false);
            //compSprite.SetActive(false);
            //whiteBox.GetComponent<WhiteFade>().FadeIn();
        }
        else if (shot == 11)
        {
            //whiteBox.GetComponent<WhiteFade>().FadeOut();
            //bg.SetActive(false);
            //hugoSprite.SetActive(false);
            //maxSprite.SetActive(false);
            //mmSprite.SetActive(false);
            //compSprite.SetActive(false);
            //whiteBox.GetComponent<WhiteFade>().FadeIn();
            boxes[12].SetActive(true);
        }
        else if (shot == 12)
        {
            boxes[12].SetActive(false);
            boxes[11].SetActive(false);
            boxes[13].SetActive(true);
        }
        else if (shot == 13)
        {
            boxes[13].SetActive(false);
            boxes[14].SetActive(true);
        }
        else if (shot == 14)
        {
            boxes[14].SetActive(false);
            boxes[15].SetActive(true);
        }
        else if (shot == 15)
        {
            boxes[15].SetActive(false);
            boxes[16].SetActive(true);
        }
        else if (shot == 16)
        {
            boxes[17].SetActive(true);
        }
        else if (shot == 17)
        {
            boxes[16].SetActive(false);
            boxes[17].SetActive(false);
            boxes[18].SetActive(true);
        }
        else if (shot == 19)
        {
            boxes[18].SetActive(false);
            boxes[19].SetActive(true);
        }
        else if (shot == 20)
        {
            boxes[19].SetActive(false);
            boxes[20].SetActive(true);
        }
        else if (shot == 21)
        {
            boxes[20].SetActive(false);
            boxes[21].SetActive(true);
        }
        else if (shot == 22)
        {
            boxes[21].SetActive(false);
            boxes[22].SetActive(true);
        }
        else if (shot == 23)
        {
            boxes[23].SetActive(true);
        }
        else if (shot == 24)
        {
            boxes[24].SetActive(true);
        }
        else if(shot == 25)
        {
            //SceneManager.LoadScene(Cus);
            CustomSceneManager.Instance.MoveToNextScene();
        }

    }
}
