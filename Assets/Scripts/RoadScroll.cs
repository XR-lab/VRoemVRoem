using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    [SerializeField] private float _offset = 10;
    [SerializeField] private float _moveTreshold = -7.5f;
    [SerializeField] private float _startingPositionZ = 2.5f;
    [SerializeField] private List<GameObject> roads = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].transform.localPosition = new Vector3(0, 0, i == 0 ? _startingPositionZ : roads[i - 1].transform.localPosition.z + _offset);
        }
    }

    private void Update()
    {
        if (roads[0].transform.localPosition.z < _moveTreshold)
        {
            MoveRoad();
        }
    }

    // Update is called once per frame
    private void MoveRoad()
    {
        GameObject movingRoad = roads[0];
        roads.Remove(movingRoad);
        movingRoad.transform.localPosition = new Vector3(0, 0, roads[roads.Count - 1].transform.localPosition.z + _offset);
        roads.Add(movingRoad);
    }
}
