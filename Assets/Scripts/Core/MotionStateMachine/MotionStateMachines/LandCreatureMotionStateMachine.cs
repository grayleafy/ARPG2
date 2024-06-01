using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFSM;
using FSM;

[Serializable]
public class GroundHitInfo
{
    [Tooltip("地面检测的起点")]
    public Transform groundRaycastOrigin;
    [Tooltip("地面检测的射线长度")]
    public float groundRaycastDistance = 0.9f;
    [Tooltip("投射的球体半径")]
    public float spereRadius = 0.25f;

    //[HideInInspector]
    public bool isHitGround = false;
    [HideInInspector]
    public RaycastHit ground;
    [HideInInspector]
    public bool isHitWater = false;
    [HideInInspector]
    public RaycastHit water;
    //todo：地面材质，是否是水等

    public void FixUpdateRayCast()
    {
        //地面
        int mask = ~(1 << LayerMask.GetMask("Water"));
        if (Physics.SphereCast(groundRaycastOrigin.position, spereRadius, Vector3.down, out ground, groundRaycastDistance, mask))
        {
            isHitGround = true;
        }
        else
        {
            isHitGround = false;
        }

        //水面
        mask = (1 << LayerMask.GetMask("Water"));
        if (Physics.SphereCast(groundRaycastOrigin.position, spereRadius, Vector3.down, out water, groundRaycastDistance, mask))
        {
            isHitWater = true;
        }
        else
        {
            isHitWater = false;
        }
    }
}

[Serializable]
public class LandCreatureMotionStateMachine : MotionStateMachine
{
    [Header("地面检测设置")]
    public GroundHitInfo hitInfo = new();

    protected override void AddAllStates()
    {
        AddState("OnGround", GetBlackboard<MotionState>("OnGround"));
        AddState("JumpAscend", GetBlackboard<MotionState>("JumpAscend"));
        AddState("Fall", GetBlackboard<MotionState>("Fall"));
        AddState("Roll", GetBlackboard<MotionState>("Roll"));
    }

    protected override void AddAllTriggers()
    {
        AddTrigger("OnGround", new InputJumpAndFootFree(), "JumpAscend");
        AddTrigger("JumpAscend", new MotionStateEnd() { state = GetState("JumpAscend") }, "Fall");
        AddTrigger("Fall", new IsOnGround() { groundHitInfo = hitInfo }, "OnGround");
        AddTrigger("OnGround", new IsNotOnGround() { groundHitInfo = hitInfo }, "Fall");
        AddTrigger("OnGround", new InputRoll(), "Roll");
        AddTrigger("Roll", new MotionStateEnd() { state = GetState("Roll") }, "OnGround");
        AddTrigger("Roll", new IsNotOnGround() { groundHitInfo = hitInfo }, "Fall");
    }

    protected override string GetDefaultStateName()
    {
        return "OnGround";
    }

    public override void FixUpdate(float dt)
    {
        //地面检测并保存
        hitInfo.FixUpdateRayCast();
        base.FixUpdate(dt);
    }

    public override void SetBlackboardInitNeedValueList()
    {
        base.SetBlackboardInitNeedValueList();
        AddInitNeedValue<MotionState>("OnGround");
        AddInitNeedValue<MotionState>("JumpAscend");
        AddInitNeedValue<MotionState>("Fall");
        AddInitNeedValue<MotionState>("Roll");
    }


}
