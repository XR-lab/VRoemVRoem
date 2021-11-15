using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{

    public class PoliceCarInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] public bool secondaryCar;
        [SerializeField] public int carPosInRow;

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
                if (secondaryCar)
                {
                    switch (carPosInRow) 
                    {
                        case 0:
                            if(transform.localPosition.x > _hitTracker.leftMapBoundary && transform.localPosition.x < _hitTracker.rightMapBoundary)
                                transform.localPosition = new Vector3(_player.transform.localPosition.x - 1.39f, this.transform.localPosition.y, this.transform.localPosition.z);
                            break;
                        case 1:
                            if (transform.localPosition.x > _hitTracker.leftMapBoundary && transform.localPosition.x < _hitTracker.rightMapBoundary)
                                transform.localPosition = new Vector3(_player.transform.localPosition.x + 1.39f, this.transform.localPosition.y, this.transform.localPosition.z);
                            break;
                    }

                }
                else
                {
                    transform.position = new Vector3(_player.transform.position.x, this.transform.position.y, this.transform.position.z);
                }
                
            }
        }
    }
}
