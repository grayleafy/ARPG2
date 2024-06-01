using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeScale : MonoBehaviour
{
    public float timeScale = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != timeScale)
        {
            Debug.Log("设置全局时间:" +  timeScale);
        }
        Time.timeScale = timeScale;
    }
}
