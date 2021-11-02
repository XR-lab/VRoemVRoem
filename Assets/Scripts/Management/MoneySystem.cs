using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XRLab.VRoem.Core;

public class MoneySystem : MonoBehaviour
{
    [Header("Money Count")]
    [SerializeField] private float maxmoney;
    [SerializeField] private float moneyLose;

    //floats to ge by anther scripts 
    public static float currentMonney;
    public static float currentMonneyToLose;
    public static float MonneyFromStart;

    [Header("Canvas")]
    [SerializeField] private Text canvasText;
    [SerializeField] private GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        currentMonney = maxmoney;
        currentMonneyToLose = moneyLose;
        MonneyFromStart = maxmoney;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMonney > -1)
        {
            canvasText.text = "Money: " + currentMonney;
        }
        else
        {
            currentMonney = 0;
            //gameOverUI.SetActive(true);
            //Time.timeScale = 0;
        }
    }

}
