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
        public float Speed { get { return _speed; } }
        private Material _clipMaterial;
        private Collider _collider;

        private void Start()
        {
            speedManager = FindObjectOfType<SpeedManager>();
            _clipMaterial = GetComponentInChildren<MeshRenderer>().material;
            _collider = GetComponentInChildren<Collider>();

        }

        private void Update()
        {
            //Move forward based on the speedmanagers speed and a multiplier for some objects like cars
            _speed = speedManager.ModifiedSpeed * _speedMultiplier;

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

       
    }
}