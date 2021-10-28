using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class VRUIInteractor : MonoBehaviour
{
    public static GameObject currentObject;
    int currentID;

    public List<string> buttonNames = new List<string>();
    private LineRenderer lineRenderer;
    private void Start()
    {
        currentObject = null;
        currentID = 0;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 100);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta);


        for(int i=0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            int id = hit.collider.gameObject.GetInstanceID();

            if(currentID != id)
            {
                currentID = id;
                currentObject = hit.collider.gameObject;

                string name = currentObject.name;

                switch (name)
                {
                    case "playAgain":
                        Debug.Log("hit playagain");
                        break;
                }
            }
        }
    }
}
