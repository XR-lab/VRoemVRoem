using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace XRLab.VRoem.Core
{


    public class ObjectHitTracker : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverUI, policarProp, policecarPropNextPosition, headLightsNextPosition, policeCarsTransform;
        
        [SerializeField] private int policeCarsToInstantiate = 2;

        public int objectHitCounter;
        public UnityEvent normalChaseEvent, firstHitEvent, secondHitEvent, thirdHitEvent, forthHitEvent, fifthHitEvent;
        private Vector3 policarPropStartLocation;

        [SerializeField] private List<GameObject> xtraPoliceCars = new List<GameObject>(); 

        private sirenLights sirenLights;
        private InGameMenu inGameMenu;

        [SerializeField] private float behindMainPoliceOffset;
        [SerializeField] private float nextToMainPoliceOffset;

        private float newPoliceCarZOffset;
        private float newPoliceCarXOffset;

        public float leftMapBoundary;
        public float rightMapBoundary;


        // Start is called before the first frame update
        void Start()
        {
            newPoliceCarZOffset = policeCarsTransform.transform.position.z - behindMainPoliceOffset;
            newPoliceCarXOffset = policarProp.transform.position.x + nextToMainPoliceOffset;

            objectHitCounter = 0;
            Time.timeScale = 1;
            policarPropStartLocation = policarProp.transform.position;

            sirenLights = FindObjectOfType<sirenLights>();
            inGameMenu = FindObjectOfType<InGameMenu>();

            if (normalChaseEvent == null) { new UnityEvent(); }
            if (firstHitEvent == null) { new UnityEvent(); }
            if (secondHitEvent == null) { new UnityEvent(); }
            if (thirdHitEvent == null) { new UnityEvent(); }
            if (forthHitEvent == null) { new UnityEvent(); }
            if (fifthHitEvent == null) { new UnityEvent(); }

            firstHitEvent.AddListener(FirstHit);
            secondHitEvent.AddListener(SecondHit);
            thirdHitEvent.AddListener(ThirdHit);
            forthHitEvent.AddListener(ForthHit);
            fifthHitEvent.AddListener(FifthHit);

            CreateXtraPoliceCars();
        }

        // Update is called once per frame
        void Update()
        {
            switch (objectHitCounter)
            {
                case 1:
                    firstHitEvent.Invoke();
                    break;
                case 2:
                    secondHitEvent.Invoke();
                    break;
                case 3:
                    thirdHitEvent.Invoke();
                    break;
                case 4:
                    forthHitEvent.Invoke();
                    break;
                case 5:
                    fifthHitEvent.Invoke();
                    break;
            }
        }

        void FirstHit()
        {
            EventCauses(headLightsNextPosition, 1);
        }

        void SecondHit()
        {
            EventCauses(policecarPropNextPosition, 2);
        }

        void ThirdHit()
        {
            EventCauses(this.gameObject, 3);
        }

        void ForthHit()
        {
            EventCauses(this.gameObject, 4);
        }

        void FifthHit()
        {
            EventCauses(gameOverUI, 4);
        }

        void CreateXtraPoliceCars()
        {
            for(int i = 0; i < policeCarsToInstantiate; i++)
            {
                GameObject go = Instantiate(policarProp);
                //go.transform.parent = policeCarsTransform.transform;
                if(i == 0)
                    go.transform.localPosition = new Vector3(1.39f, 0, policeCarsTransform.transform.position.z - 2f);
                else if(i == 1)
                    go.transform.localPosition = new Vector3(-1.39f, 0, policeCarsTransform.transform.position.z -2f);

                PoliceCarInteraction policarVariables = go.GetComponent<PoliceCarInteraction>();
                policarVariables.secondaryCar = true;
                policarVariables.carPosInRow = i;
                xtraPoliceCars.Add(go);
                xtraPoliceCars[i].SetActive(false);
            }
        }



        void MoveXtraPoliceCarsForward(GameObject policar)
        {
            policar.transform.position = Vector3.Lerp(policar.transform.position, new Vector3(policar.transform.position.x, policar.transform.position.y, policarProp.transform.position.z - 2f), 1 * Time.deltaTime);
        }

        //the things that happen when a new event is called
        void EventCauses(GameObject targetGameObject, int eventIndex)
        {
            //idle
            //zwaai licht feller
            // popo dichterbij
            // 2 popo erbij, transform voor follow x verandert
            // dede

            switch (eventIndex) 
            {
                case 1:
                    sirenLights.redLight.range = 10;
                    sirenLights.blueLight.range = 10;
                    break;

                case 2:
                    policeCarsTransform.transform.position = Vector3.Lerp(policeCarsTransform.transform.position, targetGameObject.transform.position, 1 * Time.deltaTime);
                    break;

                case 3:
                    foreach(GameObject car in xtraPoliceCars)
                    {
                        car.SetActive(true);
                        car.transform.position = Vector3.Lerp(car.transform.position, new Vector3(car.transform.position.x, car.transform.position.y, policarProp.transform.position.z - 2f), 1 * Time.deltaTime);
                    }
                    break;

                case 4:
                    targetGameObject.SetActive(true);
                    break;

                case 5:
                    
                    break;
            
            
            }


        }






    }
}
