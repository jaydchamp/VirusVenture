using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteFade : MonoBehaviour
{
    
     public Animator myAnimator;
    public void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
    }
    public void FadeIn()
    {
        myAnimator.SetBool("fadeOut", false);
        myAnimator.SetBool("fadeIn", true);
    }

    public void Still()
    {
        myAnimator.SetBool("fadeIn", false);
        myAnimator.SetBool("fadeOut", false);
    }

    public void FadeOut()
    {
        myAnimator.SetBool("fadeIn", false);
        myAnimator.SetBool("fadeOut", true);
    }


}
