using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoveWithGroundPhysics : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            transform.parent = collision.transform.parent;
        }        
    }
}
