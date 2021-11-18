using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class PositionAll : MonoBehaviour
    {
        [SerializeField] private GameObject levelMap;
        [SerializeField] private List<GameObject> playerConnectedObjects = new List<GameObject>();   

        [SerializeField] private float positionX, positionY, positionZ;
        [SerializeField] private List<Vector3> placementpositions = new List<Vector3>();
        private void Start()
        {
            FindAndAddObjects("PlayerCar");
            FindAndAddObjects("OVRCameraRig (Player)");
            FindAndAddObjects("RayHitBox");
            FindAndAddObjects("CarBounds");
            FindAndAddObjects("SpeedManager");

            PositionMapAtZero(levelMap);
            PlaceAllOtherObjects();
        }

        void FindAndAddObjects(string objectName)
        {
            GameObject go = GameObject.Find(objectName);
            playerConnectedObjects.Add(go);  
        }


        void PositionMapAtZero(GameObject map)
        {
            GameObject go = Instantiate(map, this.transform.position, map.transform.rotation);
            go.transform.parent = this.transform;
        }

        void PlaceAllOtherObjects()
        {  
            string objectName = "";
            for (int i = 0; i < playerConnectedObjects.Count; i++)
            {
                objectName = playerConnectedObjects[i].name;

                switch (objectName)
                {
                    case "PlayerCar":
                        PositionObject(playerConnectedObjects[0],this.transform.position);
                        break;

                    case "OVRCameraRig (Player)":
                        PositionObject(playerConnectedObjects[1], placementpositions[0]);
                        break;

                    case "RayHitBox":
                        PositionObject(playerConnectedObjects[2], placementpositions[1]);
                        break;

                    case "CarBounds":
                        PositionObject(playerConnectedObjects[3], placementpositions[2]);
                        break;

                    case "SpeedManager":
                        PositionObject(playerConnectedObjects[4], this.transform.position);
                        break;
                }
            }
        }

        void PositionObject(GameObject obj, Vector3 extraPositions)
        {
            if (obj == null) return;

            float diffOnX = extraPositions.x - this.transform.position.x;
            float diffOnY = extraPositions.y - this.transform.position.y;
            float diffOnZ = extraPositions.z - this.transform.position.z;



            if(extraPositions == this.transform.position)
            {
                obj.transform.position = this.transform.position;
            }
            else
            {
                obj.transform.position = new Vector3(diffOnX, diffOnY, diffOnZ);
            }
        }
    }
}

