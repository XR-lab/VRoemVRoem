using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    

    public class TrainSpawner : MonoBehaviour
    {
        public enum trainSections 
        {
            TOP,
            MIDDLE,
            BOTTOM
        };



        


        [SerializeField] private List<GameObject> trainSpawnPoints = new List<GameObject>();
        [SerializeField] private List<GameObject> leftTrains = new List<GameObject>(); 
        [SerializeField] private List<GameObject> rightTrains = new List<GameObject>();
        [SerializeField] internal int leftBorder = -15;
        [SerializeField] internal int rightBorder = 15;
        [SerializeField] private int startingTrains;
        [SerializeField] private trainSections currentWay;
        [SerializeField] private GameObject train;

        internal List<trainSections> sections = new List<trainSections>();

        private int currentLeftTrain = 0;
        private int currentRightTrain = 0;

        private GameObject SpawnedTrainsTransform;
        private Transform nextTrainSpawnTransform = null;

        private void Start()
        {
            sections.Add(trainSections.TOP);
            sections.Add(trainSections.MIDDLE);
            sections.Add(trainSections.BOTTOM);

            //train = GameObject.Find("Train");
            SpawnedTrainsTransform = GameObject.Find("SpawnedTrains");

            GameObject go = GameObject.Find("TrainSpawnPoints");
            for(int i = 0; i < go.transform.childCount; i++)
            {
                trainSpawnPoints.Add(go.transform.GetChild(i).gameObject);
            }

            SpawnTrains(train, 0);
            SpawnTrains(train, 1);

            SetTrainAtDesignatedPosition();
            CallNextTrain(1);
        }

        void SpawnTrains(GameObject train, int leftOrRight)
        {
            for(int i = 0; i < (startingTrains/2); i++)
            {
                GameObject go = Instantiate(train, transform.position, transform.rotation);
                go.transform.parent = SpawnedTrainsTransform.transform;
                TrainMovement trainVariables = go.GetComponent<TrainMovement>();
                
                switch (leftOrRight)
                {
                    case 0:
                        go.transform.localRotation = Quaternion.Euler(0, 270, 0);
                        trainVariables.leftToRight = true;
                        leftTrains.Add(go);
                        go.SetActive(false);
                        break;

                    case 1:
                        go.transform.localRotation = Quaternion.Euler(0, 90, 0);
                        trainVariables.rightToLeft = true;
                        rightTrains.Add(go);
                        go.SetActive(false);
                        break;
                }
            }
            

        }

        void TurnOnLeftTrain()
        {
            leftTrains[currentLeftTrain].gameObject.GetComponent<TrainMovement>().currentTrainWay = currentWay;
            leftTrains[currentLeftTrain].gameObject.transform.position = nextTrainSpawnTransform.position;
            leftTrains[currentLeftTrain].SetActive(true);
        }

        void TurnOnRightTrain()
        {
            rightTrains[currentRightTrain].gameObject.GetComponent<TrainMovement>().currentTrainWay = currentWay;
            rightTrains[currentRightTrain].gameObject.transform.position = nextTrainSpawnTransform.position;
            rightTrains[currentRightTrain].SetActive(true);
        }


        public void SetTrainAtDesignatedPosition()
        {
            int leftTrainorRightTrain = (int)Random.Range(0f, 2f);
            int itemIndex = 0;
            
            if(leftTrainorRightTrain == 0)
            {
                itemIndex = Random.Range(0, sections.Count);

                currentWay = sections[itemIndex];

                switch (sections[itemIndex])
                {
                    case trainSections.TOP:
                        nextTrainSpawnTransform = trainSpawnPoints[1].transform;
                        sections.Remove(trainSections.TOP);
                        CheckCurrentTrain(currentLeftTrain,0);
                        TurnOnLeftTrain();
                        break;

                    case trainSections.MIDDLE:
                        nextTrainSpawnTransform = trainSpawnPoints[0].transform;
                        sections.Remove(trainSections.MIDDLE);
                        CheckCurrentTrain(currentLeftTrain,0);
                        TurnOnLeftTrain();
                        break;

                    case trainSections.BOTTOM:
                        nextTrainSpawnTransform = trainSpawnPoints[2].transform;
                        sections.Remove(trainSections.BOTTOM);
                        CheckCurrentTrain(currentLeftTrain, 0);
                        TurnOnLeftTrain();
                        break;
                }

            }
            else if(leftTrainorRightTrain == 1)
            {
                itemIndex = Random.Range(0, sections.Count);

                currentWay = sections[itemIndex];

                switch (sections[itemIndex])
                {
                    case trainSections.TOP:
                        nextTrainSpawnTransform = trainSpawnPoints[4].transform;
                        sections.Remove(trainSections.TOP);
                        CheckCurrentTrain(currentRightTrain, 1);
                        TurnOnRightTrain();
                        break;

                    case trainSections.MIDDLE:
                        nextTrainSpawnTransform = trainSpawnPoints[3].transform;
                        sections.Remove(trainSections.MIDDLE);
                        CheckCurrentTrain(currentRightTrain, 1);
                        TurnOnRightTrain();
                        break;

                    case trainSections.BOTTOM:
                        nextTrainSpawnTransform = trainSpawnPoints[5].transform;
                        sections.Remove(trainSections.BOTTOM);
                        CheckCurrentTrain(currentRightTrain,1);
                        TurnOnRightTrain();
                        break;
                }
            }

        }

        internal void CallNextTrain(float time)
        {
            Invoke(nameof(SetTrainAtDesignatedPosition), time);
        }

        void CheckCurrentTrain(int train, int leftOrRight)
        {
            if(leftOrRight == 0)
            {
                if (train < leftTrains.Count - 1)
                {
                    currentLeftTrain++;
                }
                else
                {
                    currentLeftTrain = 0;
                }
            }
            else if(leftOrRight == 1)
            {
                if (train < rightTrains.Count - 1)
                {
                    currentRightTrain++;
                }
                else
                {
                    currentRightTrain = 0;
                }
            } 
        }
    } 
}

