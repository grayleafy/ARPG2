using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputRecorder))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BuffController))]
[RequireComponent(typeof(CharacterStats))]
public class Creature : Entity, IInput, IMotion, IBuff, IStats
{
    [SerializeReference, SubclassSelector]
    public MotionStateMachine motionStateMachine;


    private InputRecorder _inputRecorder;
    public InputRecorder InputRecorderComponent
    {
        get
        {
            if (_inputRecorder == null)
            {
                _inputRecorder = GetComponent<InputRecorder>();
            }
            return _inputRecorder;
        }
    }

    //buff
    private BuffController _buffController;
    public BuffController BuffController
    {
        get
        {
            if ( _buffController == null)
            {
                _buffController = GetComponent<BuffController>();
            }
            return _buffController;
        }
    }

    private CharacterStats _characterStats;
    public CharacterStats CharacterStats
    {
        get
        {
            if ( _characterStats == null)
            {
                _characterStats = GetComponent<CharacterStats>();
            }
            return _characterStats;
        }
    }


    public override void EntityStart()
    {
        base.EntityStart();
        motionStateMachine.Start();
    }

    public override void EntityUpdate(float dt)
    {
        base.EntityUpdate(dt);
        motionStateMachine.Update(dt);
    }

    public override void EntityFixUpdate(float dt)
    {
        base.EntityFixUpdate(dt);
        motionStateMachine.FixUpdate(dt);
    }

    public InputRecorder GetInputRecorder()
    {
        return InputRecorderComponent;
    }

    public MotionStateMachine GetMotionStateMachine()
    {
        return motionStateMachine;
    }

    public BuffController GetBuffController()
    {
        return BuffController;
    }


    public void Die()
    {
        
    }
}
