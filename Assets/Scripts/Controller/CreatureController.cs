using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatureController : MonoBehaviour
{
    public AnimationController anim;

    private Rigidbody2D thisRigidBody;

    float lastXAxis;
    Vector2 Direction;
    public float speed = 5f;//ÒÆ¶¯ËÙ¶È
    private Vector2 acceleration = Vector2.zero;
    public float maxSpeed = 6f;
    public float minSpeed = 4f;
    public float movementSmooth = 0.08f;
    public float jumpForce = 5f;
    public float dashForce = 10f;

    bool canMove = true;
    bool canFly = false;
    bool canJump = false;

    bool isRight = true;//ÊÇ·ñ³¯ÓÒ
    bool inAir = false;//ÊÇ·ñÐü¿Õ
    bool isGrounded = false;
    bool isDashing = false;

    bool isDied = false;

    public int maxHitPoint = 6;
    int hitPoint;

    public LayerMask GroundLayer;
    public Transform GroundedCheckPoint;
    const float GroundedRadius = .1f;
    private Collider2D[] groundCheckCol = new Collider2D[5];

    //UImanager ui;
    // Start is called before the first frame update
    private void Awake()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        hitPoint = maxHitPoint;
        //ui = GameObject.Find("Canvas").GetComponent<UImanager>();
        //ui.changeHP(hitPoint);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        groundedCheck();
    }
    void groundedCheck()
    {
        isGrounded = false;
        groundCheckCol = Physics2D.OverlapCircleAll(GroundedCheckPoint.position, GroundedRadius, GroundLayer);
        for (int i = 0; i < groundCheckCol.Length; i++)
        {
            if (groundCheckCol[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }
    }
    void flip()
    {
        isRight = !isRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void dash()
    {
        thisRigidBody.AddForce(Direction * dashForce, ForceMode2D.Impulse);
    }
    public void move(float xAxis, float yAxis, bool dashing)
    {
        if (canMove)
        {
            if ((xAxis > 0.1 && !isRight) || (xAxis < -0.1 && isRight))
            {
                flip();
            }
            Direction = new Vector2(xAxis, yAxis);
            if(xAxis < 0.1f && xAxis > -0.1f)
            {
                if (isRight)
                {
                    Direction = new Vector2(1f, yAxis);
                }
                else
                {
                    Direction = new Vector2(-1f, yAxis);
                }
                
            }
            if (isGrounded || canFly)
            {
                if (xAxis != 0)
                {
                    anim.run();
                }
                else if (yAxis == 0f)
                {
                    anim.idle();
                }
                canJump = true;
                if (dashing)
                {
                    isDashing = true;
                    dash();
                }
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                Vector2 newVelocity = new Vector2(xAxis * speed, thisRigidBody.velocity.y);
                thisRigidBody.velocity = Vector2.SmoothDamp(thisRigidBody.velocity, newVelocity, ref acceleration, movementSmooth);
                thisRigidBody.velocity = new Vector2(thisRigidBody.velocity.x, 0f);
            }
            else
            {
                if (yAxis == 0f)
                {
                    anim.idle();
                }
                else
                {
                    anim.jump(yAxis);
                }
            }
            if (canJump && yAxis > 0)//ÌøÔ¾
            {
                Vector2 newVelocity = new Vector2(thisRigidBody.velocity.x, yAxis * jumpForce);
                thisRigidBody.velocity = Vector2.SmoothDamp(thisRigidBody.velocity, newVelocity, ref acceleration, movementSmooth);
                canJump = false;
            }
        }
    }
    public void getHurted()
    {
        if (hitPoint > 0)
        {
            hitPoint--;
        }
        anim.hurted();
        if (hitPoint <= 0)
        {
            die();
        }
    }
    public void die()
    {
        canMove = false;
        hitPoint = 0;
        isDied = true;
        anim.die();
    }
}
