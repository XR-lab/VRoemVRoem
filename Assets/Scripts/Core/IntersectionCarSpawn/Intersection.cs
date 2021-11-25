using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{

    [SerializeField] private GameObject spawnCarobj;
    [SerializeField] private GameObject Road;

    // spawntimer
    [SerializeField] private float secondsBetweenSpawn;
    private float elapsedTime = 0.0f;

    // how much times is pressed
    private int timesPress;

    [SerializeField] private List<GameObject> spawnpoints;


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > secondsBetweenSpawn)
        {
            spawnCarObj();
            elapsedTime = 0;
        }
    }


    void spawnCarObj()
    {
        if (timesPress % 2 == 0)
        {
            GameObject clone  = Instantiate(spawnCarobj, spawnpoints[timesPress].transform.position, transform.rotation * Quaternion.Euler(spawnCarobj.transform.rotation.x, 90f, -90f));
            clone.transform.parent = this.transform;
        }
        else
        {
            GameObject clone = Instantiate(spawnCarobj, spawnpoints[timesPress].transform.position, transform.rotation * Quaternion.Euler(spawnCarobj.transform.rotation.x, -90f, -90f));
            clone.transform.parent = this.transform;
        }
        timesPress = timesPress + 1;
        if (timesPress == spawnpoints.Count)
        {
            timesPress = 0;
        }
    }
}
