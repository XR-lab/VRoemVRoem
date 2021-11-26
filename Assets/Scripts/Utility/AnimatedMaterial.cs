using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMaterial : MonoBehaviour
{
    [SerializeField] private Texture[] _occlusionArray;
    [SerializeField] private float _interval = 0.025f;
    private Material _material;

    private int _arrayIndex = 0;
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer < _interval)
        {
            return;
        }

        _timer = 0;

        if (_arrayIndex < _occlusionArray.Length)
        {
            _arrayIndex += 1;
        }
        else
        {
            _arrayIndex = 0;
        }
        _material.SetTexture("_OcclusionMap", _occlusionArray[_arrayIndex]);
    }
}
