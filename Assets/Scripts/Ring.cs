using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private float _ringSpeed = 1;
    [SerializeField] private Renderer _renderer;

    private void Update()
    {
        transform.Translate(Vector3.forward * _ringSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
           print("Hit");
           _renderer.material.color = Color.green;
       }
       
    }
}
