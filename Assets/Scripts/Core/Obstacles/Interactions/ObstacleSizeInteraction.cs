using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: fix code conventions, namespace.
public class ObstacleSizeInteraction : MonoBehaviour
{
    [SerializeField] private bool BigObject;
    [SerializeField] private Collider colider;

    protected void Interact(Collision collision)
    {
        if (!BigObject) return;
        
        print("hit");
        MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;
        this.gameObject.GetComponent<Collider>().isTrigger = true;
        colider.isTrigger = true;
    }
}
