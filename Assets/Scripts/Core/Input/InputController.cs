using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputController : MonoBehaviour
{
    InputRecorder inputRecorder;



    private void Reset()
    {

    }

    private void Start()
    {
        inputRecorder = GetComponent<InputRecorder>();
    }

    private void Update()
    {
        InputMove();
        InputJump();
        InputRoll();
        InputCommand();
    }

    void InputMove()
    {
        float up = Input.GetKey(KeyCode.W) == true ? 1.0f : 0f;
        float down = Input.GetKey(KeyCode.S) == true ? -1.0f : 0f;
        float left = Input.GetKey(KeyCode.A) == true ? -1.0f : 0f;
        float right = Input.GetKey(KeyCode.D) == true ? 1.0f : 0f;
        Vector2 move = new Vector2(left + right, up + down).normalized;
        inputRecorder.move = move;
    }

    void InputJump()
    {
        bool jump = Input.GetKey(KeyCode.Space);
        inputRecorder.jump = jump;
    }

    void InputRoll()
    {
        bool roll = Input.GetKey(KeyCode.LeftShift);
        inputRecorder.roll = roll;
    }

    void InputCommand()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (key == KeyCode.Mouse0 && EventSystem.current.IsPointerOverGameObject())
                {
                    continue;
                }
                if (Input.GetKeyDown(key))
                {
                    inputRecorder.PressKey(key);
                }
            }
        }
        var downKeys = inputRecorder.GetDownKeys();
        foreach (KeyCode key in downKeys)
        {
            if (key == KeyCode.Mouse0 && EventSystem.current.IsPointerOverGameObject())
            {
                continue;
            }
            if (Input.GetKeyUp(key))
            {
                inputRecorder.UnPressKey(key);
            }
        }
    }
}
