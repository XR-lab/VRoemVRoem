using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class MoveObstacle : MonoBehaviour
    {
        [SerializeField] private float _speedMultiplier = 1;

        private SpeedManager speedManager;
        private float _speed;

        private void Start()
        {
            speedManager = FindObjectOfType<SpeedManager>();
        }

        private void Update()
        {
            _speed = speedManager.ModifiedSpeed;

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
    }
}