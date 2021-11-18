using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    

    public class TrainSpawner : MonoBehaviour
    {
        private enum trainSections 
        {
            TOP,
            MIDDLE,
            BOTTOM
        };



        private List<trainSections> sections = new List<trainSections>();


        [SerializeField] private List<GameObject> trainSpawnPoints = new List<GameObject>();
        [SerializeField] private List<GameObject> leftTrains = new List<GameObject>(); 
        [SerializeField] private List<GameObject> rightTrains = new List<GameObject>();

        private GameObject train;
        private GameObject SpawnedTrainsTransform;


        [SerializeField] private int startingTrains;

        private void Start()
        {
            sections.Add(trainSections.TOP);
            sections.Add(trainSections.MIDDLE);
            sections.Add(trainSections.BOTTOM);

            train = GameObject.Find("Train");
            SpawnedTrainsTransform = GameObject.Find("SpawnedTrains");

            GameObject go = GameObject.Find("TrainSpawnPoints");
            for(int i = 0; i < go.transform.childCount; i++)
            {
                trainSpawnPoints.Add(go.transform.GetChild(i).gameObject);
            }

            SpawnTrains(train, 0);
            SpawnTrains(train, 1);

            
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
                        trainVariables.leftToRight = true;
                        leftTrains.Add(go);
                        go.SetActive(false);
                        break;

                    case 1:
                        trainVariables.rightToLeft = true;
                        rightTrains.Add(go);
                        go.SetActive(false);
                        break;
                }
            }
            

        }

        private void Update()
        {
            
        }

        void TurnOnTrainAtDesignatedSpawnPoint()
        {
            int leftTrainorRightTrain = (int)Random.Range(0f, 1f);

            if(leftTrainorRightTrain == 0)
            {
                //spawnleft train


            }
            else if(leftTrainorRightTrain == 1)
            {
                //spawn righttrain
            }

            //zet een trein aan op een random section
            //laat trein bewegen
            //voor het uit zetten van de trein voeg de section die hij heeft weer toe

            //random selecteer een linker of rechter trein
        }


        IEnumerator NextIncommingTrain()
        {
            yield return new WaitForSeconds(8);


        }


    }

    
}

