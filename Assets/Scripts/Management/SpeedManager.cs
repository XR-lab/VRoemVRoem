using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 2;
    [SerializeField] private float _modifiedSpeed = 1;
    [SerializeField] private float _finalSpeed = 1;
    [SerializeField] private float _roadSpeedMultiplier = 0.5f;
    [SerializeField] private float _accelerationLerpSpeed = 1;
    [SerializeField] private float _currentMultiplier = 1;

    public float FinalSpeed { get { return _finalSpeed; } }
    public float CurrentMultiplier { get { return _currentMultiplier; } }
    private float _targetSpeed = 1;

    private void Start()
    {
        _modifiedSpeed = _baseSpeed;
        _finalSpeed = _baseSpeed;
    }

    public void CalculateModifedSpeed(float multiplier)
    {
        _currentMultiplier = multiplier;

        _targetSpeed = _baseSpeed * multiplier;

        _modifiedSpeed = _targetSpeed;
    }

    public void NormalBasedSpeed(float multiplier)
    {
        float _targetSpeed = _modifiedSpeed * multiplier;

        _finalSpeed = Mathf.Lerp(_finalSpeed, _targetSpeed, _accelerationLerpSpeed * Time.deltaTime);
    }
}
