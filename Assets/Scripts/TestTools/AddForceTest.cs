using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddForceTest : MonoBehaviour
{
    public int t = 1;
    private void Start()
    {
        if (t == 1)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.VelocityChange);
            GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 dir = Vector3.forward + Vector3.up;
            GetComponent<Rigidbody>().AddForce(dir, ForceMode.VelocityChange);
        }
    }
}
