using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//TODO: Refactor, not DRY
namespace XRLab.VRoem.Core
{
    public class ObjectHitTracker : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverUI, policarProp, policecarPropNextPosition, headLightsNextPosition, policeCarsTransform;
        [SerializeField] private List<GameObject> xtraPoliceCars = new List<GameObject>();
        [SerializeField] private int policeCarsToInstantiate = 2;
        [SerializeField] private float behindMainPoliceOffset;
        [SerializeField] private float nextToMainPoliceOffset;

        private Vector3 policarPropStartLocation;
        private float newPoliceCarZOffset;
        private float newPoliceCarXOffset;
        private SirenLights sirenLights;
        private InGameMenu inGameMenu;
       
        public float leftMapBoundary;
        public float rightMapBoundary;
        public int objectHitCounter;
        public UnityEvent normalChaseEvent, firstHitEvent, secondHitEvent, thirdHitEvent, forthHitEvent, fifthHitEvent;



        // Start is called before the first frame update
        void Start()
        {
            newPoliceCarZOffset = policeCarsTransform.transform.position.z - behindMainPoliceOffset;
            newPoliceCarXOffset = policarProp.transform.position.x + nextToMainPoliceOffset;

            objectHitCounter = 0;
            policarPropStartLocation = policarProp.transform.position;

            sirenLights = FindObjectOfType<SirenLights>();
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

        }

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


            //test for changing the events in Unity (will end after final event release)
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                objectHitCounter++;
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

        //Function isn't called this push (possible in final push)
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

        //Function isn't called this push (possible in final push)
        void MoveXtraPoliceCarsForward(GameObject policar)
        {
            policar.transform.position = Vector3.Lerp(policar.transform.position, new Vector3(policar.transform.position.x, policar.transform.position.y, policarProp.transform.position.z - 2f), 1 * Time.deltaTime);
        }

        //This is the main function that gets called in the smaller event functions
        void EventCauses(GameObject targetGameObject, int eventIndex)
        {
            //idle
            //zwaai licht feller
            // popo dichterbij
            // 2 popo erbij, transform voor follow x verandert
            // dede

            switch (eventIndex) 
            {
                //First Time hit, Police lights get brighter
                case 1:
                    sirenLights.redLight.range = 10;
                    sirenLights.blueLight.range = 10;
                    break;

                //Second time hit, police car comes closer, behind the player
                case 2:
                    policeCarsTransform.transform.position = Vector3.Lerp(policeCarsTransform.transform.position, targetGameObject.transform.position, 1 * Time.deltaTime);
                    break;

                //case 4:
                //    GameObject car1 = policeCarsTransform.transform.GetChild(1).gameObject;
                //    GameObject car2 = policeCarsTransform.transform.GetChild(2).gameObject;

                //    car1.SetActive(true);
                //    car2.SetActive(true);
                //    //car1.transform.position = Vector3.Lerp(car1.transform.position, new Vector3(car1.transform.position.x, car1.transform.position.y, car1.transform.position.z - 2f), 1 * Time.deltaTime);
                //    //car2.transform.position = Vector3.Lerp(car2.transform.position, new Vector3(car2.transform.position.x, car2.transform.position.y, car2.transform.position.z - 2f), 1 * Time.deltaTime);
                //    //policeCarsTransform.transform.GetChild(1).gameObject.SetActive(true);
                //    //policeCarsTransform.transform.GetChild(2).gameObject.SetActive(true);


                //    //foreach(GameObject car in targetGameObject.transform)
                //    //{
                //    //    car.SetActive(true);
                //    //    //car.transform.position = Vector3.Lerp(car.transform.position, new Vector3(car.transform.position.x, car.transform.position.y, policarProp.transform.position.z - 2f), 1 * Time.deltaTime);
                //    //}
                //    break;

                    //Third time hit, where the gameover ui will be shown
                case 3:
                    inGameMenu.deadMenu.SetActive(true);
                    targetGameObject.SetActive(true);
                    Time.timeScale = 0;
                    break;

            }
        }
    }
}
