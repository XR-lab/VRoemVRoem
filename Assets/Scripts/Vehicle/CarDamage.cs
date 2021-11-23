using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using XRLab.VRoem.Vehicle;

//TODO: Add namespace
public class CarDamage : MonoBehaviour
{
    [SerializeField] private float _slowerAccelMultiplier = 0.5f;
    [SerializeField] private float _loseControlTime = 1;
    private SimpleMovementCar _carMovement;

    [SerializeField] public GameObject _hitParticle;
    [SerializeField] public GameObject _cashParticle;
    
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

        _hitParticle.SetActive(false);
        _hitParticle.SetActive(true);

        _cashParticle.SetActive(false);
        _cashParticle.SetActive(true);
    }

    private void ReturnControl()
    {
        _carMovement.CanMove = true;
    }

    IEnumerator ParticleDeactivate()
    {
        yield return new WaitForSeconds(2f);
    }
}
