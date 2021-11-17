using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Vehicle;

public class CarDamage : MonoBehaviour
{
    [SerializeField] private float _slowerAccelMultiplier = 0.5f;
    [SerializeField] private float _loseControlTime = 1;
    private SimpleMovementCar _carMovement;

    // Start is called before the first frame update
    void Start()
    {
        _carMovement = GetComponent<SimpleMovementCar>();
    }

    public void Damage()
    {
        _carMovement.CanMove = false;
        _carMovement.GetSpeedManager.LoseAllSpeed(_slowerAccelMultiplier);
        _carMovement.GetRigidbody.velocity = Vector3.zero;

        Invoke(nameof(ReturnControl), _loseControlTime);
    }

    private void ReturnControl()
    {
        _carMovement.CanMove = true;
    }
}
