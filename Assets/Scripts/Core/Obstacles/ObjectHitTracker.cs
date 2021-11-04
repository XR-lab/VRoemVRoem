using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace XRLab.VRoem.Core
{


    public class ObjectHitTracker : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverUI, policarProp, policecarPropNextPosition, headLightsNextPosition;

        public int objectHitCounter;
        public UnityEvent normalChaseEvent, firstHitEvent, secondHitEvent, lastHitEvent;
        private Vector3 policarPropStartLocation;

        // Start is called before the first frame update
        void Start()
        {
            objectHitCounter = 0;
            Time.timeScale = 1;
            policarPropStartLocation = policarProp.transform.position;

            if (normalChaseEvent == null) { new UnityEvent(); }
            if (firstHitEvent == null) { new UnityEvent(); }
            if (secondHitEvent == null) { new UnityEvent(); }
            if (lastHitEvent == null) { new UnityEvent(); }

            firstHitEvent.AddListener(FirstHit);
            secondHitEvent.AddListener(SecondHit);
            lastHitEvent.AddListener(LastHit);
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
                    lastHitEvent.Invoke();
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

        void LastHit()
        {
            EventCauses(gameOverUI, 1);
        }

        //the things that happen when a new event is called
        void EventCauses(GameObject targetGameObject, int eventIndex)
        {
            if (eventIndex == 1 || eventIndex == 2)
            {
                policarProp.transform.position = Vector3.Lerp(policarProp.transform.position, targetGameObject.transform.position, 1 * Time.deltaTime);
            }
            else if(eventIndex == 3)
            {
                targetGameObject.SetActive(true);
                Time.timeScale = 0;
            }


        }




    }
}
