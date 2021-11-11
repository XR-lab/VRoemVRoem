using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSizeInteraction : MonoBehaviour
{
    [SerializeField] private bool BigObject;
    [SerializeField] private Collider colider;

    protected void Interact(Collision collision) {
       
        if (BigObject) {
            print("hit");
            MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;
            this.gameObject.GetComponent<Collider>().isTrigger = true;
            colider.isTrigger = true;
        }
    }
}
