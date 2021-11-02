using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //setting Monney to start monney
    private void OnTriggerEnter(Collider other)
    {
        MoneySystem.currentMonney = MoneySystem.MonneyFromStart;
        this.gameObject.SetActive(false);
    }
}
