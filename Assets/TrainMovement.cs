using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class TrainMovement : MonoBehaviour
    {
        [Range(0f, 100f)]
        [SerializeField] private float trainSpeed;
        [SerializeField] internal bool leftToRight, rightToLeft;
        [SerializeField] internal TrainSpawner.trainSections currentTrainWay;

        private TrainSpawner trainSpawner;

        private void Start()
        {
            trainSpawner = FindObjectOfType<TrainSpawner>();
        }

        void Update()
        {
            MoveTrain();
            DisableTrain();
        }

        void MoveTrain()
        {
            if(leftToRight)
                transform.localPosition += Vector3.forward * (trainSpeed / 1000);
            if(rightToLeft)
                transform.localPosition += Vector3.forward * (-trainSpeed / 1000);
        }

        void DisableTrain()
        {
            if(leftToRight)
                if(transform.position.z > trainSpawner.rightBorder)
                {
                    this.gameObject.SetActive(false);
                    trainSpawner.sections.Add(currentTrainWay);
                    trainSpawner.CallNextTrain(1);
                }    
            if(rightToLeft)
                if (transform.position.z < trainSpawner.leftBorder)
                {
                    this.gameObject.SetActive(false);
                    trainSpawner.sections.Add(currentTrainWay);
                    trainSpawner.CallNextTrain(1);
                }
        } 
    }
}

