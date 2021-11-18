using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{

    [SerializeField] private GameObject spawnCarobj;

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
        if (timesPress > 0)
        {
            Instantiate(spawnCarobj, spawnpoints[timesPress].transform.position, transform.rotation * Quaternion.Euler(0f, -90f, 0f));
        }
        else
        {
            Instantiate(spawnCarobj, spawnpoints[timesPress].transform.position, transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        }
        timesPress = timesPress + 1;
        if (timesPress == spawnpoints.Count)
        {
            timesPress = 0;
        }
    }
}
