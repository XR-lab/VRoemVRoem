using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class DestroyAfterPos : MonoBehaviour
    {
        [SerializeField] private float _destroyPosZ = -10;
        public ObstacleGenerator _obstacleGenerator;

        private void Update()
        {
            if (transform.position.z < _destroyPosZ)
            {
                Destroy(gameObject);
            }
        }
    }
}

