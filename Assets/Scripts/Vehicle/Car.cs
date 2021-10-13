using UnityEngine;

namespace XRLab.VRoem.Vehicle
{
    public abstract class Car : MonoBehaviour
    {
        public abstract void SetOrientation(Vector3 lookAtPosition);
    }
}

