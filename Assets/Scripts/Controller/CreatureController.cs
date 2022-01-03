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
    public float speed = 8f;//ÒÆ¶¯ËÙ¶È
    private Vector2 acceleration = Vector2.zero;
    public float maxSpeed = 10f;
    public float minSpeed = 4f;
    private const float movementSmooth = 0.1f;
    private float gravity;
    public float jumpForce = 30f;
    public float dashForce = 50f;

    bool canMove = true;
    bool canFly = false;
    bool canJump = false;
    bool canHurted = true;

    bool isRight = true;//ÊÇ·ñ³¯ÓÒ
    bool inAir = false;//ÊÇ·ñÐü¿Õ
    bool isGrounded = false;
    bool isDashing = false;
    bool isOnLadder = false;

    bool isDied = false;

    public int maxHitPoint = 6;
    public int hitPoint;

    private LayerMask GroundLayer;
    public Transform[] GroundedCheckPoints;
    const float GroundedRadius = .1f;
    private Collider2D[] groundCheckCol = new Collider2D[5];

    private LayerMask LadderLayer;
    public Transform[] ClimbCheckPoints;
    const float ClimbRadius = .1f;
    private Collider2D[] ladderCheckCol = new Collider2D[5];

    public float hurtGap = 1;
    private float time = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        time = 0;
        thisRigidBody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        hitPoint = maxHitPoint;
        GroundLayer = LayerMask.GetMask("Ground");
        LadderLayer = LayerMask.GetMask("Ladder");
        gravity = thisRigidBody.gravityScale;
    }
    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        time += Time.deltaTime;
        canHurtedCheck();
        groundedCheck();
        onLadderCheck();
    }
    void canHurtedCheck()
    {
        if (time % hurtGap <= Time.deltaTime && !isDied)
        {
            canHurted = true;
        }
    }
    void groundedCheck()
    {
        isGrounded = false;
        foreach(Transform point in GroundedCheckPoints)
        {
            groundCheckCol = Physics2D.OverlapCircleAll(point.position, GroundedRadius, GroundLayer);
            for (int i = 0; i < groundCheckCol.Length; i++)
            {
                if (groundCheckCol[i].gameObject != gameObject)
                {
                    isGrounded = true;
                }
            }
        }
    }
    void onLadderCheck()
    {
        isOnLadder = false;
        thisRigidBody.gravityScale = gravity;
        foreach(Transform point in ClimbCheckPoints)
        {
            ladderCheckCol = Physics2D.OverlapCircleAll(point.position, ClimbRadius, LadderLayer);
            for (int i = 0; i < ladderCheckCol.Length; i++)
            {
                if (ladderCheckCol[i].gameObject != gameObject)
                {
                    isOnLadder = true;
                    thisRigidBody.gravityScale = 0f;
                    thisRigidBody.velocity = Vector2.zero;
                }
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
    public void rebound(float force)
    {
        thisRigidBody.AddForce(new Vector2(-Direction.x * force, -Direction.y * force + force), ForceMode2D.Impulse);
    }
    public void move(float xAxis, float yAxis, bool dashing)
    {
        if (canMove)
        {
            if(!isOnLadder && !canFly && yAxis<=0f)
            {
                yAxis = 0f;
            }
            if ((xAxis > 0.1f && !isRight) || (xAxis < -0.1f && isRight))
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
            if (isOnLadder)
            {
                anim.attack();
                canJump = false;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                thisRigidBody.velocity = new Vector2(xAxis * speed, 0);

                if (xAxis == 0f && yAxis != 0f)
                {
                    Vector2 newVelocity = new Vector2(thisRigidBody.velocity.x, yAxis * speed);
                    Vector2 curVelocity = new Vector2(thisRigidBody.velocity.x, (newVelocity.y + thisRigidBody.velocity.y) / 2);
                    thisRigidBody.velocity = Vector2.SmoothDamp(curVelocity, newVelocity, ref acceleration, movementSmooth);
                }
                if (xAxis * Direction.x < 0f && yAxis > 0f)
                {
                    if(xAxis < 1f && xAxis > 0f)
                    {
                        xAxis = 1f;
                    }
                    if (xAxis > -1f && xAxis < 0f)
                    {
                        xAxis = -1f;
                    }
                    thisRigidBody.gravityScale = gravity;
                    thisRigidBody.velocity = new Vector2(xAxis * speed, yAxis * jumpForce);
                    canJump = false;
                }
            }
            else
            {
                if (isGrounded || canFly)
                {
                    if (xAxis != 0f)
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
                    Vector2 curVelocity = new Vector2((newVelocity.x + thisRigidBody.velocity.x) / 2, thisRigidBody.velocity.y);
                    thisRigidBody.velocity = Vector2.SmoothDamp(curVelocity, newVelocity, ref acceleration, movementSmooth);
                    //thisRigidBody.velocity = new Vector2(thisRigidBody.velocity.x, 0f);
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
                    thisRigidBody.velocity = new Vector2(thisRigidBody.velocity.x, yAxis * jumpForce);
                    canJump = false;
                }
            }
        }
    }
    public bool getHurted()
    {
        bool hurted = false;
        if (canHurted)
        {
            canHurted = false;
            if (hitPoint > 0)
            {
                hitPoint--;
                hurted = true;
            }
            anim.hurted();
            if (hitPoint <= 0)
            {
                die();
            }
        }
        return hurted;
    }
    public void die()
    {
        canHurted = false;
        canMove = false;
        hitPoint = 0;
        isDied = true;
        anim.die();
    }
}
