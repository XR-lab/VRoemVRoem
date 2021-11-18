using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class TrainMovement : MonoBehaviour
    {
        [Range(0f, 100f)]
        [SerializeField] private float trainSpeed;
        [SerializeField] internal bool leftToRight, rightToLeft;

        
        void Update()
        {
            MoveTrain();
        }

        void MoveTrain()
        {
            if(leftToRight)
                transform.localPosition += Vector3.forward * (trainSpeed / 1000);
            if(rightToLeft)
                transform.localPosition += Vector3.forward * (-trainSpeed / 1000);
        }
    }
}

