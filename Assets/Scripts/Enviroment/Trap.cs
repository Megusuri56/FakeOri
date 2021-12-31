using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    bool canHurt = true;

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.GetComponentsInParent<CreatureController>()!=null && canHurt)
        {
            CreatureController[] creatures = other.collider.GetComponentsInParent<CreatureController>();
            Debug.Log(other.collider.name);
            foreach (CreatureController creature in creatures)
                creature.getHurted();
        }
    }
}
