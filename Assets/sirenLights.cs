using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XRLab.VRoem.Core
{

    public class sirenLights : MonoBehaviour
    {
        public Light redLight;
        public Light blueLight;

        // Update is called once per frame
        void Update()
        {
            RotateLight360Degrees(redLight);
            RotateLight360Degrees(blueLight);

        }

        void RotateLight360Degrees(Light light)
        {
            light.transform.Rotate(Vector3.up, 1.5f);
        }

    }
}
