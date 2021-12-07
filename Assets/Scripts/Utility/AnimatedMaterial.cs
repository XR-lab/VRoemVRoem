using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMaterial : MonoBehaviour
{
    [SerializeField] private Texture[] _albedoArray;
    [SerializeField] private Texture[] _occlusionArray;
    [SerializeField] private float _interval = 0.025f;
    [SerializeField] private float _delayTime = 0;
    private Material _material;
    private int _albedoIndex = 0;
    private int _occlusionIndex = 0;
    private float _timer = 0;
    private bool _paused = false;    

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;

        if (_delayTime > 0)
        {
            StartDelayTime();
        }
    }

    private void StartDelayTime()
    {
        _paused = true;
        Invoke(nameof(UnPause), _delayTime);
    }

    private void UnPause()
    {
        _paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_paused)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer < _interval)
        {
            return;
        }

        _timer = 0;

        if (_albedoArray.Length > 0)
        {
            if (_albedoIndex < _albedoArray.Length - 1)
            {
                _albedoIndex += 1;
            }
            else
            {
                _albedoIndex = 0;

                if (_delayTime > 0)
                {
                    StartDelayTime();
                }
            }
            _material.mainTexture = _albedoArray[_albedoIndex];
        }

        if (_occlusionArray.Length == 0)
        {
            return;
        }

        if (_occlusionIndex < _occlusionArray.Length - 1)
        {
            _occlusionIndex += 1;
        }
        else
        {
            _occlusionIndex = 0;
        }
        _material.SetTexture("_OcclusionMap", _occlusionArray[_occlusionIndex]);
        _material.SetTexture("_EmissionMap", _occlusionArray[_occlusionIndex]);
    }
}
