using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    [SerializeField] private bool repair;
    [SerializeField] private bool speedBoost;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //give effect
            WitchPowerUp();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Car")) {
            //give effect
            WitchPowerUp();
            // destroy pick up
            Destroy(this);
        }
    }

    private void WitchPowerUp() {
        if(repair == true && speedBoost == true) {
            Debug.LogError("PickUp " + gameObject.name + " heeft meer dan een soort power aan staan");
            return;
        }else if(repair == false && speedBoost == false) {
            Debug.LogError("PickUp " + gameObject.name + " heeft geen power up aan staan");
            return;
        }
        if(repair == true) {
            Repair();
        }
        if(speedBoost == true) {
            SpeedBoost();
        }
        
    }
    private void Repair() {
        //repair car
        Debug.Log("repair repair repair");
    }
    
    private void SpeedBoost() {
        //give speedboost
        Debug.Log("Speed Speed Speed");
    }
}
