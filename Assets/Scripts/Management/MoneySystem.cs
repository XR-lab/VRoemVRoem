using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XRLab.VRoem.Core;

//TODO: Fix Namespace, use set functions or set properties. Update is not needed
public class MoneySystem : MonoBehaviour
{
    [Header("Money Count")]
    [SerializeField] private float maxmoney;
    [SerializeField] private float moneyLose;

    //floats to ge by anther scripts 
    public static float currentMonney;
    public static float MonneyToLose;
    public static float MonneyFromStart;
    public static float BankMoney;

    [Header("Canvas")]
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        currentMonney = maxmoney;
        MonneyToLose = moneyLose;
        MonneyFromStart = maxmoney;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMonney > 1)
        {
            moneyText.text = "Money: " + currentMonney;
        }
        else
        {
            currentMonney = 0;
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
