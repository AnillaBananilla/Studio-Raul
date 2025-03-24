using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkills", menuName = "skill/PlayerSkills")]
public class PlayerSkills : ScriptableObject
{
    public List<Skill> skills = new List<Skill>();

    [System.Serializable]
    public class Skill
    {
        public string name;
        public bool isUnlocked;
    }
}
    

