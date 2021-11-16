using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{

    public class PoliceCarInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        public bool secondaryCar;
        public int carPosInRow;

        private List<GameObject> carsInTransform = new List<GameObject>();
        private float distanceFromCopCarInRow = 1.39f;
        private float distanceFromMainCarOnZ = 6f;
        private float distanceFromCopCarOnZ = 1.5f;
        private ObjectHitTracker _hitTracker;


        // Start is called before the first frame update
        void Start()
        {
            _hitTracker = FindObjectOfType<ObjectHitTracker>();
            _player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

            for(int i = 0; i < carsInTransform.Count; i++)
            {
                carsInTransform.Add(transform.GetChild(i).gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            FollowPlayerOnX();
        }

        void FollowPlayerOnX()
        {
            if (_player != null)
            {
                if(carsInTransform[1].activeInHierarchy == false && carsInTransform[2].activeInHierarchy == false)
                {
                    transform.position = new Vector3(_player.transform.position.x, this.transform.position.y, transform.position.z);
                }
                else
                {
                     //if(transform.position.x <= _hitTracker.leftMapBoundary)
                }
                    
            }
        }
    }
}
