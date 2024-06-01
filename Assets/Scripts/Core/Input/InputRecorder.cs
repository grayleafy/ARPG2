using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class InputCommand
{
    //public static float holdTime = 0.5f;
    public enum InputType
    {
        Tap,
        Hold,
        Unknown
    };

    public KeyCode key;
    public InputType inputType = InputType.Unknown;
    [HideInInspector]
    public float pressTime;
    [HideInInspector]
    public float unPressTime = -1;

    /// <summary>
    /// 是否一致
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool IsMatch(InputCommand command)
    {
        return key == command.key && inputType == command.inputType;
    }

    /// <summary>
    /// 根据按下和放开的时间计算输入种类
    /// </summary>
    //public void SetInputType()
    //{
    //    if (unPressTime - pressTime >= holdTime)
    //    {
    //        inputType = InputType.Hold;
    //    }
    //    else
    //    {
    //        inputType = InputType.Tap;
    //    }
    //}
}

public class InputRecorder : MonoBehaviour
{
    public RequestValue<bool> canInput = new RequestValue<bool>((x1, x2)=>x1 && x2, true);   //是否接受输入
    public float timeout = 0.5f;
    public float holdTime = 0.5f;

    public Vector2 move = Vector2.zero;
    public bool jump = false;
    public bool roll = false;

    Dictionary<KeyCode, bool> keyDic = new Dictionary<KeyCode, bool>();
    Dictionary<KeyCode, InputCommand> commandDic = new();//已经按下还未松开的键
    Queue<InputCommand> commandQueue = new();
    public enum TapType
    {
        ShortTap,
        LongTap
    }

    private void Update()
    {
        DetectHold();
        ClearTimeoutCommand();
    }

    /// <summary>
    /// 清理输入
    /// </summary>
    public void ClearInput()
    {
        move = Vector2.zero;
        jump = false;
        roll = false;
        keyDic.Clear();
        commandQueue.Clear();
        commandDic.Clear();
    }

    public List<KeyCode> GetDownKeys()
    {
        return commandDic.Keys.ToList();
    }

    #region 禁用相关
    /// <summary>
    /// 一次禁用输入请求
    /// </summary>
    public void AddDisableRequest()
    {
        canInput.AddRequest(false);
    }
    /// <summary>
    /// 取消一次禁用输入请求
    /// </summary>
    public void RemoveDisableRequest()
    {
        canInput.RemoveRequest(false);
    }


    #endregion


    #region 外部输入操作调用
    public void PressKey(KeyCode key)
    {
        if (canInput.GetResult() == true)
        {
            keyDic[key] = true;
            //Debug.Log("press " + key);
            InputCommand inputCommand = new InputCommand()
            {
                key = key,
                pressTime = Time.realtimeSinceStartup
            };
            commandDic[key] = inputCommand;
        }
    }

    public void UnPressKey(KeyCode key)
    {
        if (canInput.GetResult() == true)
        {
            keyDic[key] = false;
            //Debug.Log("unpress " + key);
            if (commandDic.ContainsKey(key))
            {
                var command = commandDic[key];
                command.unPressTime = Time.realtimeSinceStartup;
                //如果队列中还不存在该命令则加入指令队列
                if (command.inputType == InputCommand.InputType.Unknown)
                {
                    if (command.unPressTime - command.pressTime >= holdTime)
                    {
                        command.inputType = InputCommand.InputType.Hold;
                    }
                    else
                    {
                        command.inputType = InputCommand.InputType.Tap;
                    }
                    commandQueue.Enqueue(command);
                    commandDic.Remove(key);
                }
            }
        }  
    }

    public void TapKey(KeyCode key)
    {
        PressKey(key);
        UnPressKey(key);
    }

    public bool GetKey(KeyCode key)
    {
        if (keyDic.ContainsKey(key) == false)
        {
            keyDic.Add(key, false);
        }
        return keyDic[key];
    }

    public void HoldKey(KeyCode key, float keepTime)
    {
        //协程实现
        throw new System.NotImplementedException();
    }
    #endregion

    /// <summary>
    /// 获取下一个指令并且移出队列
    /// </summary>
    /// <returns></returns>
    public InputCommand DequeueInputCommand()
    {
        if (commandQueue.Count == 0)
        {
            return null;
        }
        return commandQueue.Dequeue(); ;
    }


    //清理过期的指令
    void ClearTimeoutCommand()
    {
        while (commandQueue.Count > 0)
        {
            InputCommand command = commandQueue.Peek();
            if (command.unPressTime >= 0 && command.pressTime < Time.realtimeSinceStartup - timeout)
            {
                commandQueue.Dequeue();
            }
            else
            {
                break;
            }
        }
    }

    //检测Hold
    void DetectHold()
    {
        float currentTime = Time.realtimeSinceStartup;
        foreach (var key in commandDic.Keys)
        {
            if (commandDic[key].inputType == InputCommand.InputType.Unknown && currentTime - commandDic[key].pressTime >= holdTime)
            {
                commandDic[key].inputType = InputCommand.InputType.Hold;
                commandQueue.Enqueue(commandDic[key]);
            }
        }
    }

    /// <summary>
    /// 由技能系统调用，忽略不需要的指令
    /// </summary>
    /// <param name="validKeys"></param>
    public void IgnoreInvalidTap(HashSet<KeyCode> validKeys)
    {
        throw new System.NotImplementedException();
    }






    #region 镜头转换
    /// <summary>
    /// 计算摄像头转换后的移动的世界方向
    /// </summary>
    /// <param name="moveInput"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public Vector3 TransformMoveDirection(Vector2 moveInput, Camera camera, out Quaternion rotate)
    {
        Vector3 up = Vector3.ProjectOnPlane(camera.transform.up, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;
        Vector3 dir = up * moveInput.y + right * moveInput.x;
        if (dir != Vector3.zero)
        {
            rotate = Quaternion.LookRotation(dir, Vector3.up);
        }
        else
        {
            rotate = Quaternion.identity;
        }
        return dir;
    }

    /// <summary>
    /// 想要得到目标方向，应该输入什么
    /// </summary>
    /// <param name="targetDirection"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public Vector2 InverseTransformMoveDirection(Vector3 targetDirection, Camera camera)
    {
        Vector3 up = Vector3.ProjectOnPlane(camera.transform.up, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;
        targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
        float y = Vector3.Dot(up, Vector3.Project(targetDirection, up));
        float x = Vector3.Dot(right, Vector3.Project(targetDirection, right));
        return new Vector2(x, y);
    }
    #endregion
}
