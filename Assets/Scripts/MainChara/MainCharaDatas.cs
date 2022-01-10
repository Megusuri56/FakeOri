using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharaDatas : MonoBehaviour
{
    private List<string> skillNames = new List<string>();
    
    public void addSkill(string newSkill)
    {
        if (!skillNames.Contains(newSkill))
        {
            skillNames.Add(newSkill);
        }     
    }
    public bool haveSkill(string skillName)
    {
        return skillNames.Contains(skillName);
    }
}
