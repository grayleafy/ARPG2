using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMgr : SingletonMono<CameraMgr>
{
    private List<ShakeRequest> shakeRequests = new();

    private CinemachineBrain cinemachineBrain;
    /// <summary>
    /// 主相机
    /// </summary>
    public CinemachineBrain CinemachineBrain
    {
        get
        {
            if (cinemachineBrain == null)
            {
                cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
                if (cinemachineBrain == null) cinemachineBrain = Camera.main.AddComponent<CinemachineBrain>();
            }
            return cinemachineBrain;
        }
    }
    /// <summary>
    /// 当前激活的虚拟相机
    /// </summary>
    public CinemachineVirtualCamera ActiveCamera => CinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;




    

    public void AddShakeRequest(ShakeRequest shakeRequest)
    {
        shakeRequests.Add(DeepCloner.GetInstance().DeepCopy(shakeRequest));
    }

    public void AddShakeRequest(float duration)
    {
        AddShakeRequest(new ShakeRequest() { duration = duration, amplitudeGain = 1 });
    }

    private void Update()
    {
        //时间更新并清理
        for (int i = shakeRequests.Count - 1; i >= 0; i--)
        {
            var temp = shakeRequests[i];
            temp.duration -= Time.deltaTime;
            shakeRequests[i] = temp;
            if (shakeRequests[i].duration <= 0) shakeRequests.RemoveAt(i);
        }
        //获取最大的振幅并设置
        float maxAmplitude = 0;
        for (int i = 0; i < shakeRequests.Count; i++)
        {
            maxAmplitude = Mathf.Max(maxAmplitude, shakeRequests[i].amplitudeGain);
        }
        ActiveCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = maxAmplitude;
    }
}
