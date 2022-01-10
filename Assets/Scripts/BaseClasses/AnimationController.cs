using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public virtual void run() { }
    public virtual void idle() { }
    public virtual void hurted() { }
    public virtual void jump(float ySpeed) { }
    public virtual void attack() { }
    public virtual void die() { }
}
