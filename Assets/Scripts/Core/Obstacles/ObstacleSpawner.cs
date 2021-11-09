using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XRLab.VRoem.Core
{
    public class ObstacleSpawner : MonoBehaviour
    {   //order zoals nu is ring = 0, cube = 1, vuilnisbak = 2
        [SerializeField] private List<MoveObstacle> moveableObstacles = new List<MoveObstacle>();
        [Range(1, 10)] [SerializeField] private float _spawnWidth;
        [Range(0.1f, 10)] [SerializeField] private float _spawnHeight;
        int spawnnr;

        private void Start()
        {
            // for now, use invoke repeating. Should be replaced with a custom coroutine
            InvokeRepeating("SpawnRing", 0.2f, 0.3f);
        }

        private void SpawnRing()
        {
            float randomWidth = Random.Range(-_spawnWidth / 2f, _spawnWidth / 2f) + transform.position.x;
            float randomHeight = Random.Range(-_spawnHeight / 2f, _spawnHeight / 2f) + transform.position.y;
            Vector3 spawnPosition = new Vector3(randomWidth, randomHeight, transform.position.z);
            switch (spawnnr)
            {
                default: 
                    Instantiate(moveableObstacles[0], spawnPosition, transform.rotation, transform);
                    spawnnr++;
                    break;
                case 1:
                    Instantiate(moveableObstacles[1], spawnPosition, transform.rotation, transform);
                    spawnnr++;
                    break;
                case 2:
                    Instantiate(moveableObstacles[2], spawnPosition, transform.rotation, transform);
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
