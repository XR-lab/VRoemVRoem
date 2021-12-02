using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Place Curly Bracket on new lines, Add namespace, clean-up (headers, grouping etc.)
public class SpeedManager : MonoBehaviour {
    [SerializeField] private float _baseSpeed = 2;
    [SerializeField] private float _modifiedSpeed = 1;
    [SerializeField] private float _finalSpeed = 1;
    [SerializeField] private float _roadSpeedMultiplier = 0.5f;
    [SerializeField] private float _accelerationLerpSpeed = 1;
    [SerializeField] private float _currentMultiplier = 1;
    [SerializeField] private float _lerpSpeedMutltiplierThreshold = 0.5f;
    [SerializeField] private float _speedBeforeHit = 0;
    [SerializeField] private float _timeBeforeFullAccel = 2;

    public float FinalSpeed { get { return _finalSpeed; } }
    public float SpeedBeforeHit { get { return _speedBeforeHit; } }
    public float CurrentMultiplier { get { return _currentMultiplier; } }
    private float _targetSpeed = 1;
    [SerializeField] private float _accelerationLerpMultiplier = 1;
    private bool _overwritingControllerSpeed = false;
    public bool OverwritingControllerSpeed { get { return _overwritingControllerSpeed; } }

    private void Start() {
        _modifiedSpeed = _baseSpeed;
        LoseSpeed(0.1f, 0);
    }

    public void CalculateModifedSpeed(float multiplier) {
        SetModifiedSpeed(_overwritingControllerSpeed ? _currentMultiplier : multiplier);

        if (_overwritingControllerSpeed && _modifiedSpeed > _baseSpeed * _currentMultiplier - 0.1f) {
            _overwritingControllerSpeed = false;
        }
    }

    private void SetModifiedSpeed(float multiplier) {
        //Caluclate speed based on the base speed of the manager * number of the given multiplier (normally the car)
        _currentMultiplier = multiplier;

        _modifiedSpeed = _baseSpeed * multiplier;
    }

    public void NormalBasedSpeed(float multiplier) {
        float _targetSpeed = _modifiedSpeed * multiplier;

        if (multiplier > _lerpSpeedMutltiplierThreshold) {
            _finalSpeed = Mathf.Lerp(_finalSpeed, _targetSpeed, _accelerationLerpSpeed * _accelerationLerpMultiplier * Time.deltaTime);
        } else {
            _finalSpeed = _targetSpeed;
        }
    }

    public void OverrideControllerSpeed(float multiplier, float slowerAccelMultiplier) {
        _overwritingControllerSpeed = true;
        _accelerationLerpMultiplier = slowerAccelMultiplier;
        SetModifiedSpeed(multiplier);
    }

    public void LoseSpeed(float slowerAccelMultiplier, float speedLossMultiplier) {
        _speedBeforeHit = _finalSpeed;
        _finalSpeed *= speedLossMultiplier;
        SetModifiedSpeed(1);
        _accelerationLerpMultiplier = slowerAccelMultiplier;
        Invoke(nameof(SetFullAcceleration), _timeBeforeFullAccel);
    }

    private void SetFullAcceleration()
    {
        _accelerationLerpMultiplier = 1;
    }

    public void StopSpeedingUp()
    {
        _baseSpeed = 0;
        _accelerationLerpMultiplier = 0.5f;
    }
}
