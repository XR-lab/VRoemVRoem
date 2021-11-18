using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

//TODO: Fix code conventions
namespace XRLab.VRoem.Core {

    public class InGameMenu : MonoBehaviour {

        [SerializeField] private GameObject pauzeMenu;
        [SerializeField] internal GameObject deadMenu;



        //check input 
        void Start() {
            pauzeMenu.SetActive(false);
            deadMenu.SetActive(false);
            Time.timeScale = 1;
        }


        //restart press space
        void Update() {
            if (Input.GetKeyDown("space")) {
                RestartLevel();
            }

        }

        //check if pauzemenu is active
        void CheckOpen() {
            if (!pauzeMenu.activeInHierarchy) {
                Open();
            } else if (pauzeMenu.activeInHierarchy) {
                close();
            }
        }

        // call to restart level
        public void RestartLevel() {
            Time.timeScale = 1;

            SceneManager.LoadScene(0);
        }

        public void PauzeMenuButton() {
            if (pauzeMenu.activeInHierarchy) {
                close();
            } else if (!pauzeMenu.activeInHierarchy) {
                Open();
            }
        }

        //open pauze menu and stop game time
        public void Open() {
            Time.timeScale = 0;
            pauzeMenu.SetActive(true);
            Debug.Log("open");

        }
        //close pauze menu and start game time
        public void close() {
            Time.timeScale = 1;
            pauzeMenu.SetActive(false);
            Debug.Log("close");
        }

        public void BackToMainMenu() {
            SceneManager.LoadScene(1);
        }
    }
}
