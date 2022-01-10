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
        CreatureController creature = other.collider.GetComponentInParent<CreatureController>();
        if (creature != null && canHurt)
        { 
            if (creature.getHurted())
            {
                 creature.rebound(force);
             }
        }
    }
}
