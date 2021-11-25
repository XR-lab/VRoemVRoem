using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace XRLab.VRoem.Core
{
    public class ObjectHitTracker : MonoBehaviour
    {
        [SerializeField] private GameObject  mainPoliceCar, mainPoliceCarNextPosition, policeCarsTransform;
        [SerializeField] private List<GameObject> xtraPoliceCars = new List<GameObject>();
        [SerializeField] private int policeCarsToInstantiate = 2;
        private SirenLights sirenLights;
        private InGameMenu inGameMenu;
       
        public float leftMapBoundary;
        public float rightMapBoundary;
        [SerializeField] internal int objectHitCounter;
       
        void Start()
        {
            objectHitCounter = 0;
            sirenLights = FindObjectOfType<SirenLights>();
            inGameMenu = FindObjectOfType<InGameMenu>();
            Time.timeScale = 1;

            CreateXtraPoliceCars();
        }

        void Update()
        {
            switch (objectHitCounter)
            {
                case 1:
                    EventCauses(this.gameObject, 1);
                    break;
                case 2:
                    EventCauses(mainPoliceCarNextPosition, 2);
                    break;
                case 3:
                    EventCauses(mainPoliceCarNextPosition, 3);
                    break;
                case 4:
                    EventCauses(inGameMenu.deadMenu, 4);
                    break;
            }
        }

        void CreateXtraPoliceCars()
        {
            int k = 0;
            for (int i = 0; i < policeCarsToInstantiate; i++)
            {
                GameObject go = Instantiate(mainPoliceCar);
                if(i == 0)
                    go.transform.localPosition = new Vector3(1.39f, 0, policeCarsTransform.transform.position.z - 16f);
                else if(i == 1)
                    go.transform.localPosition = new Vector3(-1.39f, 0, policeCarsTransform.transform.position.z -16f);

                PoliceCarInteraction policarVariables = go.GetComponent<PoliceCarInteraction>();
                k++;
                policarVariables.popoIndex = k;
                xtraPoliceCars.Add(go);
                xtraPoliceCars[i].SetActive(false);
            }
        }

        void MoveXtraPoliceCarsForward(GameObject policar)
        {
            policar.transform.position = Vector3.Lerp(policar.transform.position, new Vector3(policar.transform.position.x, policar.transform.position.y, mainPoliceCar.transform.position.z - 2.25f), 1 * Time.deltaTime);
        }

        void EventCauses(GameObject targetGameObject, int eventIndex)
        {
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

                //Third time hit, 2 more police cars join the pursuit
                case 3:
                    //
                    if (policeCarsTransform.transform.position != targetGameObject.transform.position)
                    {
                        policeCarsTransform.transform.position = targetGameObject.transform.position;
                    }
                    xtraPoliceCars[0].SetActive(true);
                    xtraPoliceCars[1].SetActive(true);
                    MoveXtraPoliceCarsForward(xtraPoliceCars[0]);
                    MoveXtraPoliceCarsForward(xtraPoliceCars[1]);
                    break;

                //Forth time hit, where the gameover ui will be shown
                case 4:
                    targetGameObject.SetActive(true);
                    Time.timeScale = 0;
                    break;

            }
        }
    }
}
