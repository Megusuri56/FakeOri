using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CG_Scene001_1 : CGController
{
    private CreatureController mainChara;
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
    }
}
