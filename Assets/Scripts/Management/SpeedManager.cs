using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 2;
    [SerializeField] private float _modifiedSpeed = 1;
    [SerializeField] private float _accelerationLerpSpeed = 1;
    [SerializeField] private float _currentMultiplier = 1;

    public float ModifiedSpeed { get { return _modifiedSpeed; } }
    public float CurrentMultiplier { get { return _currentMultiplier; } }
    private float _targetSpeed = 1;

    private void Start()
    {
        _modifiedSpeed = _baseSpeed;
    }

    public void CalculateModifedSpeed(float multiplier)
    {
        //Caluclate speed based on the base speed of the manager * number of the given multiplier (normally the car)
        _currentMultiplier = multiplier;

        _targetSpeed = _baseSpeed * multiplier;

        //Lerp speed to make the transition between speeds smooth
        _modifiedSpeed = Mathf.Lerp(_modifiedSpeed, _targetSpeed, _accelerationLerpSpeed * Time.deltaTime);
    }
}
