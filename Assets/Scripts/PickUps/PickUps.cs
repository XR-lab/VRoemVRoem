using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    [HideInInspector] public int ListIndex = 0;
    [HideInInspector] public string[] upgrades = new string[] { "SpeedBoost", "Repair", "Boom", "speedturn" };

     private GameObject CounterObject;
     private CollectobolCounter counter;

    private void Awake() {
        CounterObject = GameObject.FindWithTag("Counter");
        if(CounterObject == null) {
            Debug.LogError("Noo bad cant counterobject");
            return;
        }


        counter = CounterObject.GetComponent<CollectobolCounter>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //give effect
            WitchPowerUp();
            // destroy pick up
            Destroy(this);
        }
    }

    private void WitchPowerUp() {

        switch (ListIndex) {
            case 0:
                SpeedBoost();
                break;
            case 1:
                Repair();
                break;
            case 2:
                Boom();
                break;
            case 3:
                SpeedTurn();
                break;

        }
        counter.totaalCount++;
    }
    private void Repair() {
        //repair car
        Debug.Log("repair repair repair");
        counter.repairCount++;
    }
    
    private void SpeedBoost() {
        //give speedboost
        Debug.Log("Speed Speed Speed");
        counter.speedBoostCount++;
    }
    private void Boom() {
        //give boom
        Debug.Log("Boom Boom Boom");
        counter.boomCount++;
    }

    private void SpeedTurn() {
        //give Turn
        Debug.Log("Turn turn turn");
        counter.speedTurnCount++;
    }

    
}
