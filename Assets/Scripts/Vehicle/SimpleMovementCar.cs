using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public class SimpleMovementCar : Car
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lookAtThreshold = 0.1f;

        private Vector3 _lookAtPosition;

        private void Update()
        {
            if (InLookAtPositionRange()) return;

            transform.LookAt(_lookAtPosition);
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public override void SetOrientation(Vector3 lookAtPosition)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookAtPosition.x, transform.position.y, lookAtPosition.z);
            _lookAtPosition = heightCorrectedPoint;
        }

        private bool InLookAtPositionRange()
        {
            return Vector3.Distance(transform.position, _lookAtPosition) <= _lookAtThreshold;
        }
    }
}
