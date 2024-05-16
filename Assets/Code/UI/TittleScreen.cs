using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TittleScreen : MonoBehaviour
{
   
    public Animator animator;

    void Start()
    {
        animator.SetTrigger("init");
    }

    // Update is called once per frame
    void Update()
    {

    }
   
}
