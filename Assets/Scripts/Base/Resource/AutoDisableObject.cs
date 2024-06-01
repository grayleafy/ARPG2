using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableObject : PoolObject
{
    public float survivalTime;
    private float leftSurvivalTime;

    private void OnEnable()
    {
        leftSurvivalTime = survivalTime;
    }

    // Update is called once per frame
    void Update()
    {
        leftSurvivalTime -= Time.deltaTime;
        if (leftSurvivalTime < 0)
        {
            PushSelfInPool();
        }
    }
}
