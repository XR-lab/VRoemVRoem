using System.Collections;
using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Transform _handAnchor;
        [SerializeField] private float _speedMultiplierAdded = 0.2f;
        [SerializeField] private float _boostDuration = 1f;
        [SerializeField] private float _boostCooldown = 5f;
        [SerializeField] private float _fillBoostSpeed = 0.25f;
        [SerializeField] private float _boostTimer = 1;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Color _rayColor = Color.red;

        private Car _car;
        private LineRenderer _lineRenderer;
        private SpeedManager _speedManager;
        private Transform _vrCam;
        private bool _mouseControl = false;
        private bool _boosting = false;
        private bool _boostInCooldown = false;

        private void Start()
        {
            _car = GetComponent<Car>();
            _lineRenderer = GetComponent<LineRenderer>();
            _speedManager = FindObjectOfType<SpeedManager>();
            _boostTimer = _boostDuration;
            _vrCam = FindObjectOfType<OVRCameraRig>().transform;

            _fillBoostSpeed = 1 / (1 / _boostDuration * _boostCooldown);
        }

        private void Update()
        {
            ShootControlRay();
            BoostCheck();
        }

        private void ShootControlRay()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _mouseControl = !_mouseControl;
                //_vrCam.transform.localPosition = new Vector3(_vrCam.transform.localPosition.x, _mouseControl ? 3 : 3, _vrCam.transform.localPosition.z);
                _vrCam.transform.localRotation = new Quaternion(_mouseControl ? 0.16f : 0, _vrCam.transform.localRotation.y, _vrCam.transform.localRotation.z, _vrCam.transform.localRotation.w);
            }

            Ray ray = _mouseControl ? Camera.main.ScreenPointToRay(Input.mousePosition) : new Ray(_handAnchor.position, _handAnchor.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
            {
                _car.SetOrientation(hit.point, _boosting);

                if (!_boosting)
                {
                    float clampedY = (Mathf.Clamp(hit.point.y, -1, 6) + 1) / 7;

                    float multiplier = 1;

                    float upperTreshold = _mouseControl ? 0.8f : 0.4f;
                    float lowerTreshold = _mouseControl ? 0.2f : 0.1f;

                    if (clampedY < lowerTreshold)
                    {
                        multiplier -= _speedMultiplierAdded;
                    }
                    else if (clampedY > upperTreshold)
                    {
                        multiplier += _speedMultiplierAdded;
                    }

                    _speedManager.CalculateModifedSpeed(multiplier);
                }                
            }

            bool hitFound = hit.collider != null;

            _lineRenderer.SetPosition(0, _handAnchor.position);
            _lineRenderer.SetPosition(1, hitFound ? hit.point : ray.origin + ray.direction * 100);
            Debug.DrawRay(ray.origin, ray.direction * 100, _rayColor);
        }

        private void BoostCheck()
        {
            if (_boostTimer > 0)
            {
                if (Input.GetButton("Boost") || OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
                {
                    _boostTimer -= Time.deltaTime;
                    _boosting = true;
                    _speedManager.CalculateModifedSpeed(2);
                }
                else
                {
                    _boosting = false;
                    if (_boostTimer < _boostDuration)
                    {
                        _boostTimer += _fillBoostSpeed * Time.deltaTime;
                    }
                }
            }
            else if (!_boostInCooldown)
            {
                _boosting = false;
                _boostInCooldown = true;

                StartCoroutine(nameof(BoostCooldown));
            }
        }  
        
        private IEnumerator BoostCooldown()
        {
            yield return new WaitForSeconds(_boostCooldown);

            _boostInCooldown = false;
            _boostTimer = _boostDuration;
        }
    }
}