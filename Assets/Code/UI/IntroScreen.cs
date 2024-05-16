using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public GameObject titleScreen;
    public Animator animator;

    public void OnAnimationEnd()
    {
        this.gameObject.SetActive(false);
        titleScreen.SetActive(true);
    }
    public void OnAnimationStart()
    {

        animator.SetTrigger("init");
    }
}
