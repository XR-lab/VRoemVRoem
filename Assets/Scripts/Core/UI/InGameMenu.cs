using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class InGameMenu : MonoBehaviour
{
    [Header("PauzeMenu")]
    [SerializeField] private GameObject pauzeMenu;

    [Header("input")]
    private OculusInput oculusInput;


    //check input 
    void Start()
    {
        pauzeMenu.SetActive(false);

        oculusInput = GetComponent<OculusInput>();

        oculusInput.OnConfirmPress += CheckOpen;
    }


    //restart press space
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            RestartLevel();
        }

    } 
    
    //check if pauzemenu is active
    void CheckOpen()
    {
        if (!pauzeMenu.activeInHierarchy)
        {
            Open();
        }
        else if (pauzeMenu.activeInHierarchy)
        {
            close();
        }
    }

    // call to restart level
    public void RestartLevel()
    {
        Time.timeScale = 1;
        pauzeMenu.SetActive(true);
        SceneManager.LoadScene(0);
    }
   
    //open pauze menu and stop game time
    public void Open()
    {
        Time.timeScale = 0;
        pauzeMenu.SetActive(true);
        Debug.Log("open");
    }
    //close pauze menu and start game time
    public void close()
    {
        Time.timeScale = 1;
        pauzeMenu.SetActive(false);
        Debug.Log("close");
    }
}
