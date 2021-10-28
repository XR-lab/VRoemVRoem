using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private Transform _handAnchor;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Color _rayColor = Color.red;

        private Car _car;
        private LineRenderer _lineRenderer;
        private bool mouseControl = true;

        private void Start()
        {
            _car = GetComponent<Car>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.C))
            {
                mouseControl = !mouseControl;

                Ray ray = mouseControl ? Camera.main.ScreenPointToRay(Input.mousePosition) : new Ray(_handAnchor.position, _handAnchor.forward);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
                {
                    _car.SetOrientation(hit.point);
                }
                _lineRenderer.SetPosition(0, _handAnchor.position);
                _lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
                Debug.DrawRay(ray.origin, ray.direction * 100, _rayColor);
            }
            else
            {
                Ray ray = new Ray(_handAnchor.position, _handAnchor.forward);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
                {
                    _car.SetOrientation(hit.point);
                }

                _lineRenderer.SetPosition(0, _handAnchor.position);
                _lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
                Debug.DrawRay(ray.origin, ray.direction * 100, _rayColor);
            }
        }
    }

}