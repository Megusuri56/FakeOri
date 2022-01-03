using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float force = 5f;

    public bool canHurt = true;

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.GetComponentsInParent<CreatureController>()!=null && canHurt)
        {
            CreatureController[] creatures = other.collider.GetComponentsInParent<CreatureController>();
            for(int i = 0; i < creatures.Length; i++)
            {
                if (creatures[i].getHurted())
                {
                    creatures[i].rebound(force);
                }
            }    
        }
    }
}
