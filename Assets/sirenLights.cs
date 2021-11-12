using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XRLab.VRoem.Core
{

    public class sirenLights : MonoBehaviour
    {
        public Light redLight;
        public Light blueLight;
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RotateLightCircle(redLight);
            RotateLightCircle(blueLight);

        }

        void RotateLightCircle(Light light)
        {
            light.transform.Rotate(Vector3.up, 1.5f);
        }

        //void RotateLightOvale(Light light)
        //{

        //}
    }
}
