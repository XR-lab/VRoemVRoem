using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class DestroyAfterPosition : MonoBehaviour
    {
        [SerializeField] private float _positionToDestroyZ = 2;

        void Update()
        {
            if (transform.position.z < _positionToDestroyZ)
            {
                Destroy(gameObject);
            }
        }
    }
}

