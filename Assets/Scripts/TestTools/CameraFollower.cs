using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform followTarget;
    Vector3 dir;

    void Start()
    {
        dir = transform.position - followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position + dir, 0.2f);
    }
}
