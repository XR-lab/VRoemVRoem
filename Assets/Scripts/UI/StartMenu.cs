using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject statsMenu;


    public void StartGame() {
        SceneManager.LoadScene("Main");
    }

    public void StatsMenu() {
        mainMenu.SetActive(false);
        statsMenu.SetActive(true);
    }

    public void BackToMenu() {
        mainMenu.SetActive(true);
        statsMenu.SetActive(false);
    }
}
