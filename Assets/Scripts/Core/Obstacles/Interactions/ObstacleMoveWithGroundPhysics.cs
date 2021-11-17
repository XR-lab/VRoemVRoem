using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoveWithGroundPhysics : MonoBehaviour
{
    private bool parented = false;

    private void OnCollisionStay(Collision collision)
    {
        if (parented) {
            return;
        }

        if (collision.gameObject.layer == 6)
        {
            parented = true;
            transform.parent = collision.transform.parent;
        }        
    }
}
