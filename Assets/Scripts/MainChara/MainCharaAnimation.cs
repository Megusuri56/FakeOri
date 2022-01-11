using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharaAnimation : AnimationController
{
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override void idle()
    {
        anim.SetBool("isJump", false);
        anim.SetBool("isRun", false);
        //anim.SetTrigger("idle");
    }
    public override void run()
    {
        anim.SetBool("isRun", true);
    }
    public override void hurted()
    {
        anim.SetTrigger("hurt");
    }
    public override void jump(float ySpeed)
    {
        if (ySpeed > 0f && !anim.GetBool("isJump"))
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isJump", true);
        }
        if (ySpeed == 0f && anim.GetBool("isJump"))
            anim.SetBool("isJump", false);
    }
    public override void attack()
    {
        anim.SetTrigger("attack");
    }
    public override void die()
    {
        anim.SetBool("isKickBoard", false);
        anim.SetTrigger("die");
    }
}
