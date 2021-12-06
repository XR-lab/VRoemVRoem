using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class CameraFollowVRCam : MonoBehaviour
{
    private Camera _vrCam;
    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponentInParent<Camera>();
        _vrCam = GameObject.FindGameObjectWithTag(Tags.OVR).transform.GetComponentInChildren<OVRScreenFade>().GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_cam.enabled)
        {
            return;
        }

        transform.localPosition = _vrCam.transform.localPosition;
        transform.rotation = _vrCam.transform.rotation;
    }
}
