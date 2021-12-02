using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class LookAtCamera : MonoBehaviour
{
    private Transform _cam;
    [SerializeField] private bool negative = false;

    // Start is called before the first frame update
    void Start()
    {
        _cam = GameObject.FindGameObjectWithTag(Tags.OVR).transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(!negative ? _cam.position : 2 * transform.position - _cam.position);
    }
}
