using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatureController : MonoBehaviour
{
    public AnimationController anim;
    public SoundController sound;

    private Rigidbody2D thisRigidBody;

    float lastXAxis;
    Vector2 Direction;
    public float speed = 8f;//移动速度
    private Vector2 acceleration = Vector2.zero;
    public float maxSpeed = 10f;
    public float minSpeed = 4f;
    private const float movementSmooth = 0.1f;
    private const float airResistance = 1f;
    private float gravity;
    public float jumpForce = 30f;
    public float dashForce = 50f;
    [Space]
    [HideInInspector] public bool canMove = true;
    bool canFly = false;
    bool canJump = false;
    bool canHurted = true;
    [Space]
    bool isRight = true;//是否朝右
    bool isGrounded = false;
    bool isDashing = false;
    bool isOnLadder = false;
    [Space]
    bool isDied = false;
    [Space]
    public int maxHitPoint = 6;
    public int hitPoint;
    [Space]
    private LayerMask GroundLayer;
    public Transform[] GroundedCheckPoints;
    const float GroundedRadius = .1f;
    private Collider2D[] groundCheckCol = new Collider2D[5];
    [Space]
    private LayerMask LadderLayer;
    public Transform[] ClimbCheckPoints;
    const float ClimbRadius = .1f;
    private Collider2D[] ladderCheckCol = new Collider2D[5];
    [Space]
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
        //thisRigidBody.velocity = Vector2.zero;
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
                    if (thisRigidBody.velocity.y < 0f)
                    {
                        sound.land();
                    }
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
    public void flip()
    {
        isRight = !isRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void dash()
    {
        isDashing = true;
        sound.dash();
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
                sound.runEnd();
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
                    thisRigidBody.gravityScale = gravity;
                    if (xAxis != 0f)
                    {
                        sound.runStart();
                        anim.run();
                    }else 
                    {  
                        if (yAxis == 0f)
                        {
                            sound.runEnd();
                            anim.idle();
                        }
                    }
                    canJump = true;
                    if (dashing)
                    {
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
                    canJump = false;
                    if (yAxis == 0f)
                    {
                        sound.runEnd();
                        anim.idle();
                        thisRigidBody.gravityScale = gravity - airResistance;
                    }
                    else
                    {
                        sound.runEnd();
                        anim.jump(yAxis);
                    }
                }

                if (canJump && yAxis > 0)//跳跃
                {
                    sound.runEnd();
                    sound.jump();
                    thisRigidBody.velocity = new Vector2(thisRigidBody.velocity.x, yAxis * jumpForce);
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
    public void setCanMove(bool _canMove) { canMove = _canMove; }
}
