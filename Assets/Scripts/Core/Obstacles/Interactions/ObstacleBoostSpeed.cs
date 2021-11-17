using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBoostSpeed : PlayerObstacleInteraction
{
    [SerializeField] private float _speedBoost = 2.5f;
    [SerializeField] private float _slowerAccelMultiplier = 0.5f;
    [SerializeField] private bool _setPlayerForwardToMyForward = true;
    private SpeedManager _speedManager;

    private void Start()
    {
        _speedManager = FindObjectOfType<SpeedManager>();
    }

    protected override void Interact(Collider col)
    {
        _speedManager.OverrideControllerSpeed(_speedBoost, _slowerAccelMultiplier);
    }
}
