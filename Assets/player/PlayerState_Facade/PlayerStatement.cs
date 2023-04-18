using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public interface PlayerBaseState
{
    void PhysicsUpdate();//fixedupdate
    void NormalUpdate();//update
    void LeaveState();
}
#region movestate
public class StopMoveState : PlayerBaseState//静止
{
    protected Player player;
    public StopMoveState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in StopMoveState~!");
        player.StopMoving();
    }
    public void PhysicsUpdate()
    {

    }

    public void NormalUpdate()
    {

    }

    public void LeaveState()
    {

    }
}

public class StandMoveState : PlayerBaseState
{
    protected Player player;
    public StandMoveState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in StandMoveState~!");
        player.StartStand();
    }
    public void PhysicsUpdate()
    {
        player.DuringStand();
    }

    public void NormalUpdate()
    {
        player.StandInputUpdate();
    }

    public void LeaveState()
    {

    }
}

public class JumpMoveState : PlayerBaseState
{
    protected Player player;
    float jumpTime = 0.35f;
    float timer = 0f;
    public JumpMoveState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in JumpMoveState~!");
        player.StartJump();
    }
    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < jumpTime)
        {
            player.DuringJump(timer);
        }
        else
        {
           player.EndFirstJump();
        }
    }

    public void NormalUpdate()
    {
        player.JumpInputUpdate(timer);
    }

    public void LeaveState()
    {

    }
}

public class AirJumpMoveState : PlayerBaseState
{
    protected Player player;

    float jumpTime = 0.25f;
    float timer = 0f;
    public AirJumpMoveState(Player Player)
    {
        player = Player;
        player.StartAirJump();
        Debug.Log("------------------------Player in AirJumpMoveState~!");
    }
    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < jumpTime)
        {
            player.DuringAirJump(timer);
        }
        else
        {
            player.EndAirJump();
        }
    }

    public void NormalUpdate()
    {

    }

    public void LeaveState()
    {

    }
}

public class WallJumpMoveState : PlayerBaseState
{
    protected Player player;
    float jumpTime = 0.25f;
    float timer = 0f;
    public WallJumpMoveState(Player Player)
    {
        player = Player;
        player.StartWallJump();
        Debug.Log("------------------------Player in WallJumpMoveState~!");
    }
    public void PhysicsUpdate()
    {
        timer += Time.deltaTime;
        if (timer < jumpTime)
        {
            player.DuringWallJump(timer);
        }
        else
        {
            player.EndWallJump();
        }

    }

    public void NormalUpdate()
    {

    }

    public void LeaveState()
    {

    }
}

public class DashMoveState : PlayerBaseState
{
    protected Player player;
    float timer;
    float dashtime = 0.2f;
    int dir;
    public DashMoveState(Player Player)
    {
        player = Player;
        Debug.Log("------------------------Player in DashMoveState~!");
        player.StartDash();
    }
    public void PhysicsUpdate()
    {
         timer += Time.deltaTime;
        Debug.Log("------------------------����");
        if (timer < dashtime)
        {
            player.DuringDash();

        }
        else
        {
           player.EndDash();
        }
       

    }

    public void NormalUpdate()
    {

    }

    public void LeaveState()
    {

    }
}

public class FallMoveState : PlayerBaseState
{
    protected Player player;
    public FallMoveState(Player Player)
    {
        player = Player;
        player.StartFall();
        Debug.Log("------------------------Player in FallMoveState ~!");
    }
    public void PhysicsUpdate()
    {
        player.DuringFall();
    }

    public void NormalUpdate()
    {
        player.FallInputUpdate();
    }

    public void LeaveState()
    {

    }
}


#endregion
