using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    static float gap = 1.0f;
    float time = gap;

    public bool MCenter = false;
    public bool canHurt = true;

    MainCharaController mc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MCenter)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                mc.getHurted();
                time = gap;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<MainCharaController>())
        {
            mc = other.collider.GetComponent<MainCharaController>();
            MCenter = true;
            time = 0;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<MainCharaController>())
        {
            MCenter = false;
        }
    }
}
