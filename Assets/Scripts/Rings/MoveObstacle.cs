using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class MoveObstacle : MonoBehaviour
    {
        [SerializeField] private float _speedMultiplier = 1;
        [SerializeField] private float _clipSpeedMultiplier = 1;        

        private SpeedManager speedManager;
        private float _speed;
        private Material _clipMaterial;
        private Collider _collider;

        private void Start()
        {
            speedManager = FindObjectOfType<SpeedManager>();
            _clipMaterial = GetComponentInChildren<MeshRenderer>().material;
            _collider = GetComponentInChildren<Collider>();

            Vector3 clippingPlanePos = GameObject.FindGameObjectWithTag("ClippingPlane").transform.position;

            _clipMaterial.SetVector("_SectionPoint", clippingPlanePos);
        }

        private void Update()
        {
            _speed = speedManager.ModifiedSpeed;

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        //public void StartClip()
        //{
        //    StartCoroutine(nameof(ClipMaterial));
        //}

        //private IEnumerator ClipMaterial()
        //{
        //    float clipProgress = 0.7f;

        //    float colliderLengthZ = _collider.bounds.size.z;

        //    while (clipProgress > 0)
        //    {
        //        clipProgress -= Mathf.Clamp(speedManager.ModifiedSpeed * _clipSpeedMultiplier / colliderLengthZ, 0.1f, 999) * Time.deltaTime;
        //        _clipMaterial.SetFloat("_ClipProgress", clipProgress);
        //        yield return null;
        //    }
        //}
    }
}