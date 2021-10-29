using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 2;
    [SerializeField] private float _modifiedSpeed = 1;
    [SerializeField] private float _roadSpeedMultiplier = 0.5f;
    [SerializeField] private float _accelerationLerpSpeed = 1;
    [SerializeField] private float _accelerationLerpMultiplier = 1;
    [SerializeField] private float _currentMultiplier = 1;
    [SerializeField] private Material _roadMaterial;

    public float ModifiedSpeed { get { return _modifiedSpeed; } }
    public float CurrentMultiplier { get { return _currentMultiplier; } }
    private float _targetSpeed = 1;
    private bool _overwritingControllerSpeed = false;
    public bool OverwritingControllerSpeed { get { return _overwritingControllerSpeed; } }

    private void Start()
    {
        _modifiedSpeed = _baseSpeed;
    }

    public void CalculateModifedSpeed(float multiplier)
    {
        SetModifiedSpeed(_overwritingControllerSpeed ? _currentMultiplier : multiplier);

        if (_overwritingControllerSpeed && _modifiedSpeed > _baseSpeed * _currentMultiplier - 0.1f)
        {
            _overwritingControllerSpeed = false;
        }
    }

    private void SetModifiedSpeed(float multiplier)
    {
        //Caluclate speed based on the base speed of the manager * number of the given multiplier (normally the car)
        _currentMultiplier = multiplier;

        _targetSpeed = _baseSpeed * multiplier;

        //Lerp speed to make the transition between speeds smooth
        _modifiedSpeed = Mathf.Lerp(_modifiedSpeed, _targetSpeed, _accelerationLerpSpeed * _accelerationLerpMultiplier * Time.deltaTime);

        if ((_modifiedSpeed > _baseSpeed || multiplier > 1.5f) && !_overwritingControllerSpeed)
        {
            _accelerationLerpMultiplier = 1;
        }
    }

    public void OverrideControllerSpeed(float multiplier, float slowerAccelMultiplier)
    {
        _overwritingControllerSpeed = true;
        _accelerationLerpMultiplier = slowerAccelMultiplier;
        SetModifiedSpeed(multiplier);
    }

    public void LoseAllSpeed(float slowerAccelMultiplier)
    {
        _modifiedSpeed = 0;
        _accelerationLerpMultiplier = slowerAccelMultiplier;
    }
}
