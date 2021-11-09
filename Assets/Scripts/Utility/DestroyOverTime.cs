using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class DestroyOverTime : MonoBehaviour
    {
        [SerializeField] private float _destroyTime;

        void Start()
        {
            Destroy(gameObject, _destroyTime);
        }
    }
}

