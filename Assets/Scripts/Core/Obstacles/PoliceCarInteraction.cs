using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{

    public class PoliceCarInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] internal int popoIndex;

        private ObjectHitTracker _hitTracker;


        // Start is called before the first frame update
        void Start()
        {
            _hitTracker = FindObjectOfType<ObjectHitTracker>();
            _player = GameObject.FindGameObjectWithTag(Tags.PLAYER); 
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
                switch (popoIndex)
                {
                    case 0:
                        if (_player.transform.position.x < _hitTracker.rightMapBoundary && _player.transform.position.x > _hitTracker.leftMapBoundary)
                            this.transform.position = new Vector3(_player.transform.position.x, this.transform.position.y, this.transform.position.z);
                        break;

                    case 1:
                        if (_player.transform.position.x < (_hitTracker.rightMapBoundary - 1.5f) && _player.transform.position.x > (_hitTracker.leftMapBoundary + 1.5f))
                            this.transform.position = new Vector3(_player.transform.position.x - 1.5f, this.transform.position.y, this.transform.position.z);
                        break;

                    case 2:
                        if (_player.transform.position.x < (_hitTracker.rightMapBoundary - 1.5f) && _player.transform.position.x > (_hitTracker.leftMapBoundary + 1.5f))
                            this.transform.position = new Vector3(_player.transform.position.x + 1.5f, this.transform.position.y, this.transform.position.z);
                        break;
                }

            }
        }
    }
}
