using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSkills;

public class ColorUnlock : MonoBehaviour
{
    public PlayerSkills playerSkills;

    [Header("Color")]
    public string ColorName;
    public Skill UColor;
    // Start is called before the first frame update
    void Start()
    {
        UColor = playerSkills.skills.Find(skill => skill.name == ColorName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && UColor != null)
        {
            UColor.isUnlocked = true;
            Destroy(gameObject);
        }
    }
}
