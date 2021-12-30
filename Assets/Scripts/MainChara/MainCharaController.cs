using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharaController : MonoBehaviour
{
    float xAxisInput = 0f;//ˮƽ����
    float lastXInput;
    float yAxisInput;//��ֱ����
    public float speed = 5f;//�ƶ��ٶ�
    public float maxSpeed = 6f;
    public float minSpeed = 4f;
    Vector2 newPosition;//�ƶ����λ��

    bool canInput = true;

    bool isRight = true;//�Ƿ���
    bool inAir = false;//�Ƿ�����

    bool isDied = false;

    public static int maxHitPoint = 6;
    int hitPoint = maxHitPoint;
    MainCharaAnimation anim;
    //UImanager ui;
    // Start is called before the first frame update
    void Start()
    {
        quit();
        anim = GameObject.Find("CollegeStudent").GetComponent<MainCharaAnimation>();
        //ui = GameObject.Find("Canvas").GetComponent<UImanager>();
        //ui.changeHP(hitPoint);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        quit();
        input();
        beforeMove();
        move();
        die();
    }
    void input()
    {
        if (canInput)
        {
            lastXInput = xAxisInput;
            xAxisInput = Input.GetAxis("Horizontal");
            yAxisInput = Input.GetAxis("Vertical");
            if (yAxisInput < 0)
            {
                yAxisInput = 0;
            }
        }
    }
    void beforeMove()
    {
        Vector2 _speed = new Vector2(0, 0);
        if (speed < maxSpeed && xAxisInput != 0)
        {
            speed += 1f;
            _speed.x = xAxisInput * speed;
        }
        else if (speed >= minSpeed && lastXInput != 0 && xAxisInput == 0)
        {
            speed -= 1f;
            _speed.x = lastXInput * speed;
        }
        else
        {
            _speed.x = xAxisInput * speed;
        }
        if (!inAir)
        {
            _speed.y = yAxisInput * speed;
        }
        else
        {

        }
        if ((xAxisInput < -0.1f && isRight) || (xAxisInput > 0.1f && !isRight))
        {
            flip();
        }
        anim.jump(_speed.y);
        newPosition = _speed * Time.deltaTime;
    }
    void flip()
    {
        isRight = !isRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void move()
    {
        transform.Translate(newPosition, Space.World);
    }
    public void getHurted()
    {
        if (hitPoint > 0)
        {
            hitPoint--;
        }
        anim.hurted();
    }
    void die()
    {
        if (hitPoint <= 0)
        {
            anim.die();
            //SceneManager.LoadScene("SceneDie");
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
