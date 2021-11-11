using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XRLab.VRoem.Core
{


    public class RoadScroll : MonoBehaviour
    {
        [SerializeField] private float _offset = 10;
        [SerializeField] private float _moveTreshold = -7.5f;
        [SerializeField] private float _startingPositionZ = 2.5f;
        [SerializeField] private List<GameObject> roads = new List<GameObject>();

        [SerializeField] private GameObject checkpoint;
        [SerializeField] private int roadsTillCheckpoint;
        private int roadCount;

        private void Start()
        {
            //Set starting positions of the roads
            for (int i = 0; i < roads.Count; i++)
            {
                roads[i].transform.localPosition = new Vector3(0, 0, i * _offset + _startingPositionZ);
            }
        }

        private void Update()
        {
            //Check if the road in front has a lower Z position as the threshold to teleport it to the back
            if (roads[0].transform.localPosition.z < _moveTreshold)
            {
                MoveRoad();
                roadCount++;
            }

            if ( checkpoint != null && roadCount == roadsTillCheckpoint) {
                SetCheckPoint();
            }
        }

        //Teleports the font road to the last road + the offset given
        private void MoveRoad()
        {
            GameObject movingRoad = roads[0];
            roads.Remove(movingRoad);
            movingRoad.transform.localPosition = new Vector3(0, 0, roads[roads.Count - 1].transform.localPosition.z + _offset);
            roads.Add(movingRoad);
        }

        private void SetCheckPoint() {
            GameObject checkpointObj = Instantiate(checkpoint, new Vector3(roads[3].transform.position.x, (roads[3].transform.position.y - 1) + checkpoint.transform.localScale.y, roads[3].transform.position.z), Quaternion.identity);
            checkpointObj.transform.parent = roads[3].transform;
            roadCount = 0;
        }
    }
}
