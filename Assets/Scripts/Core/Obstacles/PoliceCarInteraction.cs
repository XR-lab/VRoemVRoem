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

        // Start is called before the first frame update
        void Start()
        {
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
                            if(transform.position.x > -2.5f && transform.position.x < 2.5f)
                                transform.position = new Vector3(_player.transform.position.x - 1.39f, this.transform.position.y, this.transform.position.z);
                            break;
                        case 1:
                            if (transform.position.x > -2.5f && transform.position.x < 2.5f)
                                transform.position = new Vector3(_player.transform.position.x + 1.39f, this.transform.position.y, this.transform.position.z);
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
