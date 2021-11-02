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
        [SerializeField] private MoveObstacle _vuilnisbak;
        [SerializeField] private MoveObstacle _spijkermat;
        [SerializeField] private MoveObstacle _pion;
        [Range(1, 10)] [SerializeField] private float _spawnWidth;
        [Range(0.1f, 10)] [SerializeField] private float _spawnHeight;
        float spawnnr;

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

            spawnnr = Random.Range(1 , 4);

            switch (spawnnr)
            {
                default:
                    Instantiate(_ring, spawnPosition, transform.rotation, transform);
                    break;
                case 1:
                    Instantiate(_vuilnisbak, spawnPosition, transform.rotation, transform);
                    break;
                case 2:
                    Instantiate(_pion, spawnPosition, transform.rotation, transform);
                    break;
                case 3:
                    Instantiate(_spijkermat, spawnPosition, transform.rotation, transform);
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
