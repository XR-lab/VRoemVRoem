using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XRLab.VRoem.Core;
using XRLab.VRoem.Vehicle;

//TODO: Add namespace
public class CarDamage : MonoBehaviour
{
    [SerializeField] private float _slowerAccelMultiplier = 0.5f;
    [SerializeField] private float _loseControlTime = 1;
    [SerializeField] private float _invincibilityTime = 2;
    [SerializeField] private int _health = 3;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private float _fadeSpeed = 3;
    private SimpleMovementCar _carMovement;

    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private GameObject _cashParticle;
    [SerializeField] private GameObject _carModel;
    [SerializeField] private AudioSource _carDamageAudio;
    [SerializeField] private Image _healthImage;

    private bool _invincible = false;
    private bool _blinking = false;
    private int _oldHealth = 3;
    ObjectHitTracker _hitTracker;

    void Start()
    {
        Color color = Color.white;
        color.a = 0;
        _healthImage.color = color;
        _health = _maxHealth;
        _hitTracker = FindObjectOfType<ObjectHitTracker>();
        _carMovement = GetComponent<SimpleMovementCar>();
    }

    public void Damage(float speedLossMultiplier)
    {
        if (_invincible)
        {
            return;
        }

        _oldHealth = _health;

        _carMovement.CanMove = false;
        _hitTracker.objectHitCounter += 1;
        _health -= 1;

        if (_health > 0)
        {
            _carMovement.GetRigidbody.velocity = Vector3.zero;
            _carMovement.GetSpeedManager.LoseSpeed(_slowerAccelMultiplier, speedLossMultiplier);
            Invoke(nameof(MakeVunerable), _invincibilityTime);
            Invoke(nameof(ReturnControl), _loseControlTime);
        }
        else
        {
            _carMovement.GetRigidbody.velocity = Vector3.zero;
            _carMovement.GetSpeedManager.LoseSpeed(_slowerAccelMultiplier, 0.5f);
            _carMovement.GetSpeedManager.StopSpeedingUp();
            StartCoroutine(nameof(StopMovementWhenNoSpeed));
        }

        _invincible = true;
        _blinking = true;
        StopCoroutine(nameof(BlinkCar));
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
        _blinking = false;
        _invincible = false;
    }

    private IEnumerator BlinkCar()
    {
        _healthImage.color = Color.white;

        while (_blinking)
        {
            _carModel.SetActive(false);
            UpdateHealth(_health);
            yield return new WaitForSeconds(0.2f);
            _carModel.SetActive(true);
            UpdateHealth(_oldHealth);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        UpdateHealth(_health);
        yield return new WaitForSeconds(0.5f);

        float alpha = 1;

        while(_healthImage.color.a > 0)
        {
            alpha -= _fadeSpeed * Time.deltaTime;
            _healthImage.color = new Color(_healthImage.color.r, _healthImage.color.g, _healthImage.color.b, alpha);
            yield return null;
        }        
    }

    private void UpdateHealth(float h)
    {
        _healthImage.fillAmount = h / (float)_maxHealth;
    }

    private IEnumerator StopMovementWhenNoSpeed()
    {
        while (_carMovement.GetSpeedManager.FinalSpeed > 2)
        {
            yield return null;
        }

        _carMovement.CanMove = false;
        _blinking = false;
    }

    IEnumerator ParticleDeactivate()
    {
        yield return new WaitForSeconds(2f);
    }
}
