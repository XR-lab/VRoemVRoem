using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionCarSpawn : MonoBehaviour
{
    [SerializeField] private GameObject spawnLocation ;
    [SerializeField] private GameObject spawnCarobj;
    [SerializeField] private int nrLocations;

    public Vector3 previewTrans;
    public Vector3 RoadLenght;

    // witch car toe spawn
    public bool spawnCar;

    
    // how much times is pressed
    public int timesPress;

    // spacebetween spawnpoints
    private float spacebetween;

    // spawntimer
    public float secondsBetweenSpawn;
    private float elapsedTime = 0.0f;


    public List<Vector3> spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        spacebetween = (RoadLenght.z / (nrLocations + 1));
        SpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (spawnCar)
        {
            spawnCarObj();
        }
        else if (elapsedTime > secondsBetweenSpawn)
        {
            spawnCarObj();
            elapsedTime = 0;
        }
        
    }

    void SpawnPoints()
    {
       // if ((RoadLenght.z) > previewTrans.z)
        {
            for (int i = 0; i < nrLocations; i++)
            {
                if (previewTrans.z < transform.position.z)
                {
                    previewTrans = new Vector3(previewTrans.x, previewTrans.y, previewTrans.z + spacebetween);
                    Instantiate(spawnLocation, previewTrans, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                    spawnpoints.Add(previewTrans);
                    Debug.Log(RoadLenght.z);
                    Debug.LogError(previewTrans.z);
                }
            }
        }
    }

    void spawnCarObj()
    {
        Instantiate(spawnCarobj, spawnpoints[timesPress], transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        spawnCar = false;
        timesPress = timesPress + 1;
        if (timesPress == spawnpoints.Count)
        {
            timesPress = 0;
        }
    }

    void spawnLeft()
    {

    }
}
