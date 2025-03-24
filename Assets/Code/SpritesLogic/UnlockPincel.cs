using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSkills;

public class UnlockPincel : MonoBehaviour
{
    public PlayerSkills playerSkills; 
    private Skill pincelSkill;
    public bool canAttack;

    void Start()
    {
        pincelSkill = playerSkills.skills.Find(skill => skill.name == "Pincel");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && pincelSkill != null)
        {
            pincelSkill.isUnlocked = true;
            CheckSkillUnlock();
            Destroy(gameObject);
        }
    }
    public void CheckSkillUnlock()
    {
        Skill pincelSkill = playerSkills.skills.Find(skill => skill.name == "Pincel");
        if (pincelSkill != null && pincelSkill.isUnlocked)
        {
            canAttack = true;
        }
    }
}