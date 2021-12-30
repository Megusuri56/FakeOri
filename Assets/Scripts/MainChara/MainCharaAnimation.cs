using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharaAnimation : MonoBehaviour
{
    float xAxisInput;//水平输入
    float yAxisInput;//垂直输入S

    bool inAir = false;//是否悬空

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        input();
        beforeMove();
    }
    void input()
    {
        xAxisInput = Input.GetAxis("Horizontal");
        yAxisInput = Input.GetAxis("Vertical");
        if (yAxisInput < 0)
        {
            yAxisInput = 0;
        }
    }
    void beforeMove()
    {
        anim.SetBool("isRun", false);
        if (xAxisInput != 0 && !anim.GetBool("isJump"))
            anim.SetBool("isRun", true);
        if (!inAir)
        {
        }
        else
        {

        }
    }
    public void hurted()
    {
        anim.SetTrigger("hurt");
    }
    public void jump(float ySpeed)
    {
        if (ySpeed > 0.1f && !anim.GetBool("isJump"))
            anim.SetBool("isJump", true);
        if (ySpeed < 0.1f && anim.GetBool("isJump"))
            anim.SetBool("isJump", false);
    }
    public void die()
    {
        anim.SetBool("isKickBoard", false);
        anim.SetTrigger("die");
    }
}
