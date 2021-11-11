using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //setting Monney to start monney
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            print("player");
            MoneySystem.BankMoney = MoneySystem.BankMoney + MoneySystem.currentMonney;
            MoneySystem.currentMonney = MoneySystem.MonneyFromStart;
            this.gameObject.SetActive(false);
        }
    }
}
