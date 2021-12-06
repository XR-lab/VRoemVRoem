using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManager : MonoBehaviour
{
    [SerializeField] private int _chaseLevel = 0;
    [SerializeField] private PoliceChase _middlePoliceCar;
    [SerializeField] private Transform _sidePoliceCars;
    [SerializeField] private PoliceChase _leftPoliceCar;
    [SerializeField] private PoliceChase _rightPoliceCar;
    [SerializeField] private float _minDistance = 0.5f;
    [SerializeField] private float _lerpSpeed = 2;
    [SerializeField] private Vector3 _middlePoliceCarChasePos;
    [SerializeField] private Vector3 _sidePoliceCarsChasePos;

    private Vector3 _targetPoint;
    private Transform _currentLerpingCar;
    private bool _setMiddleTargetPosZ = true;
    private bool _canChase = true;
    private int _damageWhileCantChaseCount = 0;
    private Vector3 _middleCarStartPoint;
    private Vector3 _sideCarsStartPoint;
    private int _prevChaseLevel = 0;
    private bool _doneLerping = false;

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
        FindObjectOfType<CarDamage>().OnDamage += IncreaseChaseLevel;

        _sidePoliceCars.gameObject.SetActive(false);

        _middlePoliceCar.TargetZ = _middleCarStartPoint.z;
        _leftPoliceCar.TargetZ = _sideCarsStartPoint.z;
        _rightPoliceCar.TargetZ = _sideCarsStartPoint.z;
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
                    _currentLerpingCar = _middlePoliceCar.transform;
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

        Debug.Log("REPLACING");
        _canChase = false;

        Debug.Log(repMiddle);

        _middlePoliceCar = repMiddle;
        _leftPoliceCar = repLeft;
        _rightPoliceCar = repRight;
    }
}
