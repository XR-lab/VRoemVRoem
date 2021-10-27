using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauzeMenu;
    private OculusInput oculusInput;

    void Start()
    {
        pauzeMenu.SetActive(false);

        oculusInput = GetComponent<OculusInput>();

        oculusInput.OnConfirmPress += CheckOpen;
    }


    // Update is called once per frame
    void Update()
    {
        
       
        if (Input.GetKeyDown("space"))
        {
            RestartLevel();
        }

    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        pauzeMenu.SetActive(true);
        SceneManager.LoadScene("Restartlevel");
    }
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
    public void Open()
    {
        Time.timeScale = 0;
        pauzeMenu.SetActive(true);
        Debug.Log("open");
    }
    public void close()
    {
        Time.timeScale = 1;
        pauzeMenu.SetActive(false);
        Debug.Log("close");
    }
}
