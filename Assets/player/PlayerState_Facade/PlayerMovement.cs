using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Player bindplayer;
    public Vector3 Velocity;
    public Vector3 AddVelocity = Vector3.zero;
    public bool[] skill = new bool[4];

    [Header("posChaeck")]
    public Rigidbody2D rigidbody2;
    [SerializeField] Transform groundCheck, headCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround, whatIsHead;
    const float groundedRadius = .2f;

    [Header("bool")]
    public bool nearRightWall = false;
    public bool nearLeftWall = false;
    public bool faceRight = true;
    public bool onGround = true;
    public bool onSlope = false;
    public bool canAirJump = true;
    public bool canFall = false;
    public bool canDash = true;
    public bool canMove = true;
    public bool canJumpOnAir = false;
    public bool endJumpEarly = false;

    [Header("time")]
    public float lastJumpPressed;
    public float coyoteTime;
    public float jumpBuffer = 0.2f;
    public float firstJumpEnd;
    public float lastNearWallTime;
    public float airJumpBuffer = 0.3f;

    [Header("walk&run")]
    public float accel, decel;
    public float runSpeedNow, runSpeedMax;
    public float addRunSpeed;
    public float horizontalMove = 0f;
    public float verticleMove = 0f;

    [Header("slope")]
    public float slopeAngle, slopeAngleOld, slopeAngleMax;
    public Vector2 slopeNormal;

    [Header("dash")]
    public float dashspeed;
    public int dashdir;


    [Header("jump")]
    public AnimationCurve jumpCurve, airJumpCurve;
    public float jumpForce;
    public float addJumpForce;

    public float jumpBonus = 0f;

    public int walljumpdir;

    [Header("fall")]
    [SerializeField] private LayerMask whatIsWall;
    public float fallSpeedWall, fallSpeedMax, fallSpeedAccel, addFallSpeed, changeFallSpeedMax;
    public float xWallSpeed;

    #region Input
    public void GatherJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            lastJumpPressed = Time.time;
    }
    public void GatherDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (canMove && (canDash && skill[Global.DashSkill]))
            {
                bindplayer.SetPlayerState(new DashMoveState(bindplayer));
            }
    }
    public void GatherJumpEndInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            endJumpEarly = true;
    }

    public void GatherMoveInput(InputAction.CallbackContext context)
    {

        horizontalMove = context.ReadValue<Vector2>().x;
        verticleMove = context.ReadValue<Vector2>().y;

        if (Mathf.Abs(horizontalMove) < 0.2)
            horizontalMove = 0;
        if (Mathf.Abs(verticleMove) < 0.2)
            verticleMove = 0;
    }

    #endregion

    #region Check
    public void PosCheck()
    {
        CheckGround();
        CheckWall();
        CalcuHorizontalMove();
    }
    public void CheckGround()
    {
        onGround = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        if (colliders.Length > 0)
        {
            onGround = true;
            if (bindplayer.GetState() != typeof(StandMoveState)
               && canFall)
            {
                if ((lastNearWallTime + 0.02f <= Time.time) || AddVelocity.magnitude <= 0)
                {
                    bindplayer.Land();
                }
            }
        }
        else
        {
            if (bindplayer.GetState() != typeof(FallMoveState))
            {
                if (bindplayer.GetState() == typeof(StandMoveState))
                {
                    coyoteTime = Time.time;
                }
                if (canFall)
                    bindplayer.Fall();
            }
        }

    }
    public void CheckWall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, groundedRadius, whatIsWall);
        if (colliders.Length > 0)
        {
            if (transform.localScale.x > 0)
            {
                nearRightWall = true;
                nearLeftWall = false;
            }
            else
            {
                nearRightWall = false;
                nearLeftWall = true;
            }
            lastNearWallTime = Time.time;
            return;
        }
    }
    public bool CheckHead()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(headCheck.position, 0.1f, whatIsHead);
        if (colliders.Length > 0)
        {
            if (bindplayer.GetState() == typeof(JumpMoveState))
                firstJumpEnd = Time.time;
            Velocity.y = 0;
            canFall = true;
            return true;
        }
        return false;
    }
    public void CheckSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position,
            Vector2.down, 0.3f, whatIsGround);

        if (hit)
        {
            if (horizontalMove != 0 || !canMove)
            {
                slopeNormal = Vector2.Perpendicular(hit.normal).normalized;
                slopeAngle = Vector2.SignedAngle(Vector2.up, hit.normal);
                if (slopeAngle != slopeAngleOld)
                {
                    if (slopeAngle == 0f)
                        onSlope = false;
                    else
                        onSlope = true;
                }
                slopeAngleOld = slopeAngle;

                Debug.DrawRay(hit.point, hit.normal, Color.red);
                Debug.DrawRay(hit.point, slopeNormal, Color.blue);

            }

        }
    }

    #endregion

    #region Horizontal
    public void Horizontal()
    {
        if (horizontalMove > 0)
        {
            if (!faceRight)
            {
                faceRight = true;
                Flip();
            }

        }
        else if (horizontalMove < 0)
        {
            if (faceRight)
            {
                faceRight = false;
                Flip();
            }
        }
    }
    public void Flip()
    {
        float x;
        if (faceRight)
            x = Mathf.Abs(transform.localScale.x) * 1;
        else
            x = Mathf.Abs(transform.localScale.x) * -1;
        Vector3 scal = new Vector3(x,
            transform.localScale.y, transform.localScale.z);
        transform.localScale = scal;
    }
    public void CalcuHorizontalMove()
    {
        if (horizontalMove != 0)
        {
            runSpeedNow += horizontalMove * accel * Time.fixedDeltaTime;
            runSpeedNow = Mathf.Clamp(runSpeedNow, -(runSpeedMax + addRunSpeed), runSpeedMax + addRunSpeed);

        }
        else
        {
            if (runSpeedNow != 0)
            {
                runSpeedNow = Mathf.MoveTowards(runSpeedNow, 0, decel * Time.fixedDeltaTime);
            }
        }
        Horizontal();
    }
    public void Run()
    {
        CheckSlope();
        if (!onSlope && onGround)
        {
            Velocity.x = runSpeedNow;
            //Debug.Log("onNormalGround");
        }
        else if (onSlope && Mathf.Abs(slopeAngle) < slopeAngleMax)
        {
            Velocity.x = runSpeedNow * slopeNormal.x * -1;
            Velocity.y = runSpeedNow * slopeNormal.y * -1;
            if (horizontalMove == 0)
            {
                Velocity.x = rigidbody2.velocity.x - AddVelocity.x;
                Velocity.y = rigidbody2.velocity.y - AddVelocity.y;
            }
            //Debug.Log("onSlope");
        }
        else
        {
            // Debug.Log("can't climb Slope");
            Velocity.x = rigidbody2.velocity.x - AddVelocity.x;
            Velocity.y = rigidbody2.velocity.y - AddVelocity.y;
        }


    }
    public void Dashing()
    {
        Velocity.x = dashdir * dashspeed;
        Velocity.y = 0;
    }

    public void StartDash()
    {
        canFall = false;
        canDash = false;
        if (faceRight)
            dashdir = 1;
        else
            dashdir = -1;
        StartCoroutine(AddJumpForceTime(10, 0.6f));
        StartCoroutine(AddRunSpeedTime(10, 0.6f));
    }

    public void EndDash()
    {
        canFall = true;
        runSpeedNow = 0;
        Velocity = Vector2.zero;
    }
    public void StartStand()
    {
        Velocity.y = 0;
        canDash = true;
        canAirJump = true;
        canFall = true;
        canJumpOnAir = false;
    }
    public void StandInputUpdate()
    {
        if (lastJumpPressed + jumpBuffer > Time.time && skill[Global.JumpSkill])
        {
            //待定添加跳跃效果？
            bindplayer.SetPlayerState(new JumpMoveState(bindplayer));
            return;
        }
    }
    #endregion

    #region Verticle
    public void Falling()
    {
        Velocity.x = runSpeedNow;
        if (lastNearWallTime + 0.1f > Time.time)
        {
            float wallfallmax=fallSpeedWall;
            if (verticleMove < 0)
            {
                wallfallmax = wallfallmax * (1 + Mathf.Abs(verticleMove)*0.5f);
            }
            if (Mathf.Abs(Velocity.y) > wallfallmax)
            {
                Velocity.y = -wallfallmax;
                return;
            }
        }
        else
        {
            float fallmax=fallSpeedMax + changeFallSpeedMax;
            if (verticleMove < 0)
            {
                fallmax = fallmax * (1 + Mathf.Abs(verticleMove)*0.7f);
            }
            if (Mathf.Abs(Velocity.y) > fallmax)
            {
                Velocity.y = -fallmax;
                return;
            }
        }

        if (Mathf.Abs(Velocity.y) < (fallSpeedMax + changeFallSpeedMax) / 8)
            Velocity.y -= (fallSpeedAccel + addFallSpeed) / 2 * Time.deltaTime;
        else
            Velocity.y -= (fallSpeedAccel + addFallSpeed) * Time.deltaTime;


    }

    public void FallInputUpdate()
    {
        if (lastJumpPressed + 0.02f > Time.time)
        {
            if (coyoteTime + jumpBuffer > Time.time && skill[Global.JumpSkill])//      
            {
                bindplayer.SetPlayerState(new JumpMoveState(bindplayer));
                return;
            }
            Debug.Log("get KeyCode.Space!");
        }

        if (lastJumpPressed + jumpBuffer > Time.time)
        {
            if (canJumpOnAir && skill[Global.JumpSkill])
            {
                bindplayer.SetPlayerState(new JumpMoveState(bindplayer));
                return;
            }
            if (horizontalMove == 0)
            {
                if (canAirJump && firstJumpEnd + airJumpBuffer > Time.time
                 && skill[Global.AirJumpSkill])//      
                {
                    bindplayer.SetPlayerState(new AirJumpMoveState(bindplayer));
                    return;
                }
            }
            else
            {
                if (lastNearWallTime + 0.1f > Time.time && skill[Global.WallJumpSkill])
                {
                    bindplayer.SetPlayerState(new WallJumpMoveState(bindplayer));
                    return;
                }

                if (canAirJump && firstJumpEnd + airJumpBuffer > Time.time
                 && skill[Global.AirJumpSkill])//      
                {
                    bindplayer.SetPlayerState(new AirJumpMoveState(bindplayer));
                    return;
                }
            }
        }
    }

    #endregion

    #region Jump

    public void StartJump()
    {
        endJumpEarly = false;
        canFall = false;
        canJumpOnAir = false;
        jumpBonus = Mathf.Abs(runSpeedNow) / runSpeedMax * 2;
        lastJumpPressed = 0;
    }

    public void EndFirstJump()
    {
        canFall = true;
        firstJumpEnd = Time.time;
    }

    public void Jump(float timer)
    {

        if (timer > 0.1f && endJumpEarly)
        {
            Debug.Log("endJumpEarly");
            Velocity.y = 0;
            EndFirstJump();
            return;
        }
        Velocity.x = runSpeedNow;
        Velocity.y = Mathf.Lerp(0, jumpForce + jumpBonus + addJumpForce, jumpCurve.Evaluate(timer));
        CheckHead();

    }

    public void JumpInputUpdate(float timer)
    {
        if (lastJumpPressed + 0.02 >= Time.time && timer > 0.15f)
        {
            if (canJumpOnAir && skill[Global.JumpSkill])
            {
                bindplayer.SetPlayerState(new JumpMoveState(bindplayer));
                return;
            }
            if (skill[Global.AirJumpSkill] && canAirJump)
                bindplayer.SetPlayerState(new AirJumpMoveState(bindplayer));
        }
    }

    public void StartAirJump()
    {
        canFall = false;
        canAirJump = false;
    }

    public void AirJump(float timer)
    {
        Velocity.x = runSpeedNow;
        Velocity.y = Mathf.Lerp(0, jumpForce + addJumpForce, airJumpCurve.Evaluate(timer));
        CheckHead();
    }

    public void EndAirJump()
    {
        canFall = true;
    }

    public void StartWallJump()
    {
        canFall = false;
        endJumpEarly = false;
        runSpeedNow = 0;
        if (nearLeftWall)
            walljumpdir = 1;
        else
            walljumpdir = -1;
    }

    public void WallJump(float timer)
    {
        if (timer > 0.1f)
        {
            if (endJumpEarly)
            {
                Debug.Log("endJumpEarly");
                Velocity.y = 0;
                canFall = true;
                return;
            }
        }
        Velocity.x = xWallSpeed * walljumpdir;
        Velocity.y = 0.7f * (jumpForce + addJumpForce);
        CheckHead();
    }

    #endregion

    #region Ienum

    public IEnumerator AddJumpForceTime(float add, float time)
    {
        addJumpForce += add;
        yield return new WaitForSeconds(time);
        addJumpForce -= add;
    }
    public IEnumerator AddRunSpeedTime(float add, float time)
    {
        addRunSpeed += add;
        yield return new WaitForSeconds(time);
        addRunSpeed -= add;
    }
    public IEnumerator AddFallSpeedTime(float add, float time)
    {
        addFallSpeed += add;
        yield return new WaitForSeconds(time);
        addFallSpeed -= add;
    }
    public IEnumerator AddFallSpeedMaxTime(float add, float time)
    {
        changeFallSpeedMax += add;
        yield return new WaitForSeconds(time);
        changeFallSpeedMax -= add;
    }

    #endregion
    public void StopMoving()
    {
        Velocity = Vector3.zero;
    }

    public void Moving()
    {
        rigidbody2.velocity = Velocity + AddVelocity;
    }



    //    private void OnDrawGizmos()
    // {
    //     // Bounds
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(groundCheck.position, groundedRadius);
    //     Gizmos.DrawSphere(wallCheck.position, groundedRadius);
    //     Gizmos.DrawSphere(headCheck.position, 0.1f);
    // }

}
