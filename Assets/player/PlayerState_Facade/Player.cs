using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>//facade、singleton
{
    [SerializeField]PlayerMovement playerMovement;
    [SerializeField]PlayerVisual playerVisual;
    [SerializeField]PlayerHealth playerHealth;
    [SerializeField]PlayerBuff playerBuff;

    PlayerBaseState state;

    public string statement;//debug用

    public void SetPlayerState(PlayerBaseState newState)
    {
        state.LeaveState();
        state = newState;
    }


    public System.Type GetState()
    {
        return state.GetType();
    }
    private void Start()
    {
        state = new StopMoveState(this);
        StartCoroutine(StartcoolDown());

    }

    public IEnumerator StartcoolDown(float time = 3f)//避免初始跳跃
    {
        yield return new WaitForSeconds(time);
        state = new StandMoveState(this);
    }

    public void Update()
    {
        state.NormalUpdate();
        statement = state.GetType().ToString();
    }

    private void FixedUpdate()
    {
        playerMovement.PosCheck();
        state.PhysicsUpdate();
        playerMovement.Moving();
    }

    public void StopMoving()
    {
        playerMovement.StopMoving();
    }

    public void Land()
    {
        state = new StandMoveState(this);
    }

    public void Fall()
    {
        state = new FallMoveState(this);
    }

    #region  stand 

    public void StartStand()
    {
        playerMovement.StartStand();
        playerVisual.PlayGroundAnimation
            (Mathf.Abs(playerMovement.runSpeedNow));
    }

    public void DuringStand()
    {
        playerMovement.Run();
        playerVisual.PlayGroundAnimation
            (Mathf.Abs(playerMovement.runSpeedNow));
    }
    public void StandInputUpdate()
    {
        playerMovement.StandInputUpdate();
    }
    #endregion


    #region jump
    public void StartJump()
    {
        playerMovement.StartJump();
        playerVisual.PlayJumpAnimation();
    }
    

    public void DuringJump(float timer)
    {
        playerMovement.Jump(timer);
    }

    public void EndFirstJump()
    {
        playerMovement.EndFirstJump();
    }

    public void JumpInputUpdate(float timer)
    {
        playerMovement.JumpInputUpdate(timer);
    }

    public void StartAirJump()
    {
        playerMovement.StartAirJump();
        playerVisual.PlayJumpAnimation();
    }

    public void DuringAirJump(float timer)
    {
        playerMovement.AirJump(timer);
    }

    public void EndAirJump()
    {
        playerMovement.EndAirJump();
    }

      public void StartWallJump()
    {
        playerMovement.StartWallJump();
        playerVisual.PlayJumpAnimation();
    }

    public void DuringWallJump(float timer)
    {
        playerMovement.WallJump(timer);
    }

    public void EndWallJump()
    {
        playerMovement.EndAirJump();
    }

    #endregion


    #region  fall
    public void StartFall()
    {
        playerVisual.PlayFallAnimation();
    }

    public void DuringFall()
    {
        playerMovement.Falling();
    }

    public void FallInputUpdate()
    {
        playerMovement.FallInputUpdate();
    }

    #endregion


    #region dash
    public void StartDash()
    {
        playerMovement.StartDash();
        playerVisual.PlayDashAnimation();
    }
    public void DuringDash()
    {
        playerMovement.Dashing();
    }

    public void EndDash()
    {
        playerMovement.EndDash();
    }
    #endregion



}
