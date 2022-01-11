using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CG_Scene001_1 : CGController
{
    private CreatureController mainChara;
    private bool flip = true;
    public GameObject npc_001;
    [HideInInspector] public float xAxis;
    public BoxCollider2D nextLevel;
    void Awake()
    {
        mainChara = GameObject.FindWithTag("Player").GetComponent<CreatureController>();
    }
    public override void beforeCG()
    {
        nextLevel.isTrigger = true;
        xAxis = 0f;
    }
    public override void CGgoing()
    {
        mainChara.move(xAxis, 0f, false);
        if (flip && npc_001.transform.position.x - mainChara.gameObject.transform.position.x<0.01f)
        {
            mainChara.flip();
            flip = false;
        }
    }
    public void changeSpeed()
    {
        xAxis -= 0.1f;
        if(xAxis > 0f)
        {
            Invoke("changeSpeed", 0.1f);
        }
        else
        {
            xAxis = 0f;
        } 
    }
}
