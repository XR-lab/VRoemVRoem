using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;
using XRLab.VRoem.Vehicle;

public class PoliceChase : MonoBehaviour
{
    private enum PoliceCarPos { LEFT, MIDDLE, RIGHT};

    [SerializeField] private float _minBoundsX = -3;
    [SerializeField] private float _maxBoundsX = 3;
    [SerializeField] private float _lerpSpeedX = 1;
    [SerializeField] private float _lerpSpeedZ = 1;
    [SerializeField] private float _boostLerpSpeedZ = 1;
    [SerializeField] private float _distanceBetweenCars = 2;
    [SerializeField] private float _playerBoostModifierZ = 2;
    [SerializeField] private PoliceCarPos _carPos;
    [SerializeField] private PoliceChase _replacementMiddle;
    [SerializeField] private PoliceChase _replacementLeft;
    [SerializeField] private PoliceChase _replacementRight;
    [SerializeField] private float _yPos = -0.3f;
    [SerializeField] private GameObject _sirens;
    [SerializeField] private AudioSource _brakeSfx;

    private Transform _player;
    private Transform _map;
    private PoliceManager _policeManager;
    private CarController _playerCarController;
    private Animator _anim;
    private bool _stopDriving = false;

    public float TargetZ { get; set; } = 0;
    public bool Lerping { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        _sirens.SetActive(true);
        _policeManager = FindObjectOfType<PoliceManager>();
        _player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        _playerCarController = _player.GetComponent<CarController>();
        _map = GameObject.FindGameObjectWithTag("Map").transform;
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Lerping || _stopDriving)
        {
            return;
        }

        switch (_carPos)
        {
            case PoliceCarPos.LEFT:
                _maxBoundsX = _policeManager.MiddlePoliceCar.position.x - _distanceBetweenCars;
                break;
            case PoliceCarPos.MIDDLE:
                _minBoundsX = _policeManager.LeftPoliceCar.position.x + _distanceBetweenCars;
                _maxBoundsX = _policeManager.RightPoliceCar.position.x - _distanceBetweenCars;
                break;
            case PoliceCarPos.RIGHT:
                _minBoundsX = _policeManager.MiddlePoliceCar.position.x + _distanceBetweenCars;
                break;
        }

        Vector3 targetPos = Vector3.zero;
        targetPos.x = Mathf.Clamp(_player.position.x, _minBoundsX, _maxBoundsX);
        targetPos.z = TargetZ;

        float lerpSpeedZ = _lerpSpeedZ;

        if (_playerCarController._boosting)
        {
            targetPos.z -= _playerBoostModifierZ;
            lerpSpeedZ = _boostLerpSpeedZ;
        }

        Vector3 lerpPos = transform.position;
        lerpPos.x = Mathf.Lerp(lerpPos.x, targetPos.x, _lerpSpeedX * Time.deltaTime);
        lerpPos.y = _yPos;
        lerpPos.z = Mathf.Lerp(lerpPos.z, targetPos.z, lerpSpeedZ * Time.deltaTime);

        transform.position = lerpPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StopPolice"))
        {
            StopDriving();

            if (_carPos == PoliceCarPos.MIDDLE)
            {
                _policeManager.SetReplacements(_replacementMiddle, _replacementLeft, _replacementRight);
            }            
        }
    }

    public void StopDriving()
    {
        if (_anim == null)
        {
            return;
        }

        _stopDriving = true;
        _anim.SetTrigger("Brake");
        _brakeSfx.Play();
        transform.SetParent(_map);
    }
}
