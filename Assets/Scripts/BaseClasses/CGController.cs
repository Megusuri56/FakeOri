using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGController : MonoBehaviour
{
    private bool onCG = false;
    private PlayerInput player;
    public void CGstart()
    {
        beforeCG();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        player.setCanInput(false);
        onCG = true;
    }
    void FixedUpdate()
    {
        if (onCG)
        {
            CGgoing();
        }
    }
    public virtual void beforeCG() { }
    public virtual void CGgoing() { }
    public virtual void afterCG() { }
    public void CGend()
    {
        onCG = false;
        player.setCanInput(true);
        afterCG();
    }
}
