using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CreatureController controller;

    float xAxisInput;//水平输入
    float yAxisInput;//垂直输入
    bool dashing;

    bool canInput = true;

    // Start is called before the first frame update
    void Start()
    {
        quit();
    }

    // Update is called once per frame
    void Update()
    {
        quit();
        if (canInput)
        {
            input();
        }
    }
    void FixedUpdate()
    {
        controller.move(xAxisInput, yAxisInput, dashing);
        dashing = false;
    }
    void input()
    {
        xAxisInput = Input.GetAxis("Horizontal");
        yAxisInput = Input.GetAxisRaw("Vertical");
        if (yAxisInput <= 0)
        {
            yAxisInput = 0;
            if (Input.GetButtonDown("Dash"))
            {
                dashing = true;
            }
        }
    }
    void quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
