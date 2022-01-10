using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_001 : CGController
{
    public CreatureController creatureController;
    public CG_Scene001_1 mainCharaInCG;
    private float xAxis = 1f;

    public override void CGgoing()
    {
        creatureController.move(xAxis, 0f, false);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("CheckPoint2"))
        {
            xAxis = 0f;
        }
        else if (col.gameObject.name.Equals("CheckPoint1"))
        {
            mainCharaInCG.xAxis = 1f;
        }
    }
}
