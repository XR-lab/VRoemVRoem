using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class PositionAll : MonoBehaviour
    {
        [SerializeField] private GameObject playerCar, playerVR, raycastHitBox, carBounds, speedManager, levelMap;

        [SerializeField] private float positionX, positionY, positionZ;
        private bool mapPlaced;
        private void Start()
        {
            playerCar = GameObject.Find("PlayerCar");
            playerVR = GameObject.Find("OVRCameraRig (Player)");
            raycastHitBox = GameObject.Find("RayHitBox");
            carBounds = GameObject.Find("CarBounds");
            speedManager = GameObject.Find("SpeedManager");

            PositionMapAtZero(levelMap);

            if (!mapPlaced)
            {
                //position all other objects
            }

        }


        void PositionMapAtZero(GameObject map)
        {
            GameObject go = Instantiate(map, new Vector3(transform.position.x - 7.5f, transform.position.y + 7.5f, transform.position.z - 125f), Quaternion.identity);
            go.transform.parent = this.transform;


            mapPlaced = true;
        }



    }
}

