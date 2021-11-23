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
    [SerializeField] private float _invincibilityTime = 2;
    private SimpleMovementCar _carMovement;

    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private GameObject _cashParticle;
    [SerializeField] private GameObject _carModel;
    [SerializeField] private AudioSource _carDamageAudio;

    private bool _invincible = false;

    void Start()
    {
        _carMovement = GetComponent<SimpleMovementCar>();
    }

    public void Damage()
    {
        if (_invincible)
        {
            return;
        }

        _carMovement.CanMove = false;
        _carMovement.GetSpeedManager.LoseAllSpeed(_slowerAccelMultiplier);
        _carMovement.GetRigidbody.velocity = Vector3.zero;

        Invoke(nameof(ReturnControl), _loseControlTime);
        Invoke(nameof(MakeVunerable), _invincibilityTime);

        _invincible = true;
        StartCoroutine(nameof(BlinkCar));

        if (_hitParticle == null || _cashParticle == null)
        {
            return;
        }

        _hitParticle.SetActive(false);
        _hitParticle.SetActive(true);

        _cashParticle.SetActive(false);
        _cashParticle.SetActive(true);

        _carDamageAudio.Play();
    }

    private void ReturnControl()
    {
        _carMovement.CanMove = true;
    }

    private void MakeVunerable()
    {
        _invincible = false;
    }

    private IEnumerator BlinkCar()
    {
        while(_invincible)
        {
            _carModel.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _carModel.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ParticleDeactivate()
    {
        yield return new WaitForSeconds(2f);
    }
}
