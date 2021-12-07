using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class PoliceManager : MonoBehaviour
{
    [SerializeField] private Transform[] _extraPoliceSpawns;
    [SerializeField] private int _chaseLevel = 0;
    [SerializeField] private PoliceChase _middlePoliceCar;
    [SerializeField] private Transform _sidePoliceCars;
    [SerializeField] private ExtraPoliceLerp[] _extraPoliceCars;
    [SerializeField] private PoliceChase _leftPoliceCar;
    [SerializeField] private PoliceChase _rightPoliceCar;
    [SerializeField] private float _minDistance = 0.5f;
    [SerializeField] private float _lerpSpeed = 2;
    [SerializeField] private float _delayBetweenExtraPolice = 1;
    [SerializeField] private float _extraPoliceCarBounds = 6;
    [SerializeField] private float _toNextExtraCarSpawnZ = 4;
    [SerializeField] private Vector3 _middlePoliceCarChasePos;
    [SerializeField] private Vector3 _sidePoliceCarsChasePos;

    private Vector3 _targetPoint;
    private Transform _playerCar;
    private bool _setMiddleTargetPosZ = true;
    private bool _canChase = true;
    private int _damageWhileCantChaseCount = 0;
    private Vector3 _middleCarStartPoint;
    private Vector3 _sideCarsStartPoint;
    private int _prevChaseLevel = 0;
    private bool _doneLerping = false;
    private int _extraPoliceSpawnIndex = 0;
    private int _currentExtraPoliceCar = 0;

    public Transform MiddlePoliceCar { get { return _middlePoliceCar.transform; } }
    public Transform LeftPoliceCar { get { return _leftPoliceCar.transform; } }
    public Transform RightPoliceCar { get { return _rightPoliceCar.transform; } }

    public Vector3 MiddlePoliceCarChasePos { get { return _middlePoliceCarChasePos; } }
    public Vector3 SidePoliceCarsChasePos { get { return _sidePoliceCarsChasePos; } }

    // Start is called before the first frame update
    void Start()
    {
        _middleCarStartPoint = _middlePoliceCar.transform.position;
        _sideCarsStartPoint = _sidePoliceCars.position;

        CarDamage carDamage = FindObjectOfType<CarDamage>();

        carDamage.OnDamage += IncreaseChaseLevel;
        carDamage.OnGameOver += CallInExtraPolice;

        _sidePoliceCars.gameObject.SetActive(false);

        _middlePoliceCar.TargetZ = _middleCarStartPoint.z;
        _leftPoliceCar.TargetZ = _sideCarsStartPoint.z;
        _rightPoliceCar.TargetZ = _sideCarsStartPoint.z;

        _playerCar = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
    }

    private void IncreaseChaseLevel()
    {
        _prevChaseLevel = _chaseLevel;
        _chaseLevel++;

        if (_canChase)
        {
            CheckChaseLevel();
        }    
    }

    private void Update()
    {
        if (_playerCar.position.z + _toNextExtraCarSpawnZ > _extraPoliceSpawns[_extraPoliceSpawnIndex].position.z)
        {
            if (_extraPoliceSpawnIndex < _extraPoliceSpawns.Length - 1)
            {
                _extraPoliceSpawnIndex++;
            }            
        }
    }

    private void CheckChaseLevel()
    {
        switch (_chaseLevel)
        {
            case 0:
                _middlePoliceCar.TargetZ = _middleCarStartPoint.z;
                break;
            case 1:
                if (_prevChaseLevel == 0)
                {
                    _middlePoliceCar.TargetZ = _middlePoliceCarChasePos.z;
                }
                else
                {
                    _leftPoliceCar.TargetZ = _sideCarsStartPoint.z;
                    _rightPoliceCar.TargetZ = _sideCarsStartPoint.z;
                }               
                break;
            case 2:
                _sidePoliceCars.gameObject.SetActive(true);
                _leftPoliceCar.transform.SetParent(null);
                _leftPoliceCar.enabled = true;
                _rightPoliceCar.transform.SetParent(null);
                _rightPoliceCar.enabled = true;
                _middlePoliceCar.TargetZ = _middlePoliceCarChasePos.z;
                _leftPoliceCar.TargetZ = _sidePoliceCarsChasePos.z;
                _rightPoliceCar.TargetZ = _sidePoliceCarsChasePos.z;
                break;
        }
    }

    public void CallInExtraPolice()
    {
        ExtraPoliceLerp extraPoliceCar = _extraPoliceCars[_currentExtraPoliceCar];

        Vector3 target = _playerCar.position;
        target.y = extraPoliceCar.transform.position.y;

        if (_canChase)
        {
            target.z += 4;
        }
        else
        {
            target.z = _rightPoliceCar.transform.position.z + 4;
        }

        if (_currentExtraPoliceCar == 1)
        {
            if (_playerCar.position.x > -_extraPoliceCarBounds)
            {
                target.x -= 2;
            }
            else
            {
                target.z += 4;
            }
        }

        if (_currentExtraPoliceCar == 2)
        {
            if (_playerCar.position.x < _extraPoliceCarBounds)
            {
                target.x += 2;
            }
            else
            {
                target.z += 4;
            }
        }

        extraPoliceCar.transform.position = _extraPoliceSpawns[_extraPoliceSpawnIndex].position;
        extraPoliceCar.targetPlayerPos = target;
        extraPoliceCar.gameObject.SetActive(true);

        if (_currentExtraPoliceCar < _extraPoliceCars.Length - 1)
        {
            _currentExtraPoliceCar += 1;

            Invoke(nameof(CallInExtraPolice), _delayBetweenExtraPolice);
        }
        else
        {
            Intersection[] intersections = FindObjectsOfType<Intersection>();

            for (int i = 0; i < intersections.Length; i++)
            {
                intersections[i].enabled = false;
            }
        }
    }
    
    public void ActivateReplacements()
    {        
        _middlePoliceCar.transform.SetParent(null);        
        _middlePoliceCar.enabled = true;
        
        CheckChaseLevel();
        
        _canChase = true;
    }

    public void SetReplacements(PoliceChase repMiddle, PoliceChase repLeft, PoliceChase repRight)
    {
        if (repMiddle == null)
        {
            return;
        }

        _canChase = false;

        _middlePoliceCar = repMiddle;
        _leftPoliceCar = repLeft;
        _rightPoliceCar = repRight;
    }
}
