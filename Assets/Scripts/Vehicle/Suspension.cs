using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public WheelCollider wc;

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 wheelPosition;
        if (Physics.Raycast(transform.position, -transform.up, out hit, wc.radius + wc.suspensionDistance))
        {
            wheelPosition = hit.point + wc.transform.up * wc.radius;
        }
        else
        {
            wheelPosition = wc.transform.position - wc.transform.up * wc.suspensionDistance;
        }

        transform.position = wheelPosition;
    }
}
