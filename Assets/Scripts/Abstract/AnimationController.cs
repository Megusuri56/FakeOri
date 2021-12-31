using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    public abstract void run();
    public abstract void idle();
    public abstract void hurted();
    public abstract void jump(float ySpeed);
    public abstract void die();
}
