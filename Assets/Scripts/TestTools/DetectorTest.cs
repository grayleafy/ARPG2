using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorTest : MonoBehaviour
{
    [SerializeReference, SubclassSelector, DrawHitDetectRange]
    HitExcutor hitDetector;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
