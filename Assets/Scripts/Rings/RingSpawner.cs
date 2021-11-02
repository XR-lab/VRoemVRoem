using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XRLab.VRoem.Core
{
    public class RingSpawner : MonoBehaviour
    {
        [SerializeField] private MoveObstacle _ring;
        [SerializeField] private MoveObstacle _cube;
        [SerializeField] private MoveObstacle _vuilensbak;
        [Range(1, 10)] [SerializeField] private float _spawnWidth;
        [Range(0.1f, 10)] [SerializeField] private float _spawnHeight;
        int spawnnr;

        private void Start()
        {
            // for now, use invoke repeating. Should be replaced with a custom coroutine
            InvokeRepeating("SpawnRing", 1, 1);
        }

        private void SpawnRing()
        {
            float randomWidth = Random.Range(-_spawnWidth / 2f, _spawnWidth / 2f) + transform.position.x;
            float randomHeight = Random.Range(-_spawnHeight / 2f, _spawnHeight / 2f) + transform.position.y;
            Vector3 spawnPosition = new Vector3(randomWidth, randomHeight, transform.position.z);
            switch (spawnnr)
            {
                default: 
                    Instantiate(_ring, spawnPosition, transform.rotation, transform);
                    spawnnr++;
                    break;
                case 1:
                    Instantiate(_cube, spawnPosition, transform.rotation, transform);
                    spawnnr++;
                    break;
                case 2:
                    Instantiate(_vuilensbak, spawnPosition, transform.rotation, transform);
                    spawnnr = 0;
                    break;
            }
           
           
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(_spawnWidth, _spawnHeight, 1f));
        }
    }
}
