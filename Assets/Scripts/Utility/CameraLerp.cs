using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private float _minDistance = 0.1f;
    [SerializeField] private LayerMask _nothingLayer;
    [SerializeField] private LayerMask _everythingLayer;

    private bool _moveCamera = false;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private Camera _cam;
    private Camera _vrCam;

    // Start is called before the first frame update
    void Start()
    {
        _vrCam = GameObject.FindGameObjectWithTag(Tags.OVR).transform.GetComponentInChildren<OVRScreenFade>().GetComponent<Camera>();
        transform.position = _vrCam.transform.position;
        _cam = GetComponentInChildren<Camera>();
        _startPos = transform.position;
        _cam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_moveCamera)
        {
            return;
        }

        _targetPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, _targetPos, _lerpSpeed * Time.deltaTime);
        transform.rotation = _vrCam.transform.rotation;

        if (Vector3.Distance(transform.position, _targetPos) < _minDistance)
        {
            _moveCamera = false;

            if (_targetPos == _startPos)
            {
                _cam.enabled = false;
                _vrCam.cullingMask = _everythingLayer;
            }
        }
    }

    public void MoveToStart()
    {
        MoveCameraToPos(_startPos);
    }

    public void MoveCameraToPos(Vector3 pos)
    {
        _moveCamera = true;
        _vrCam.cullingMask = _nothingLayer;
        _cam.enabled = true;
        _targetPos = pos;
    }
}
