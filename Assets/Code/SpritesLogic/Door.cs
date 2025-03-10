using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameManager gameManagerX;
    public GameObject myObject; // Arrastra el objeto desde el Inspector

    public Animator anim;
    void Start()
    {
        gameManagerX = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player") & GameManager.instance.HasKey())
        {
            anim.SetTrigger("trigger");
            SetObjectActive(myObject);
        }
    }
    public void SetObjectActive(GameObject obj)
    {
        obj.SetActive(false);
    }
}

