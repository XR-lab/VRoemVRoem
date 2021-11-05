using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XRLab.VRoem.Core
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private MoveObstacle _ring;
        [Range(1, 10)] [SerializeField] private float _spawnWidth;
        [Range(0.1f, 10)] [SerializeField] private float _spawnHeight;

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

            Instantiate(_ring, spawnPosition, transform.rotation, transform);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(_spawnWidth, _spawnHeight, 1f));
        }
    }
}
