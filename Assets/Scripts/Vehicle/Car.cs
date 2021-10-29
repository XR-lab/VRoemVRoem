using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public abstract class Car : MonoBehaviour
    {
        public abstract void SetTargetPoint(Vector3 lookAtPosition, bool boosting);
    }
}

