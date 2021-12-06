using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;
using XRLab.VRoem.Vehicle;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _overridenPositionMultiplierX = 0;
    private CameraLerp _cam;
    private CarController _carController;

    // Start is called before the first frame update
    void Start()
    {
        _cam = FindObjectOfType<CameraLerp>();
        _carController = FindObjectOfType<CarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag(Tags.PLAYER))
        {
            if (_target != null)
            {
                _cam.MoveCameraToPos(_target.position);         
            }
            else
            {
                _cam.MoveToStart();
            }
        }
    }

}
