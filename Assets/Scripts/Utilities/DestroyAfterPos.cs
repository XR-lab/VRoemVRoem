using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class DestroyAfterPos : MonoBehaviour
    {
        [SerializeField] private float _destroyPosZ = -10;
        public ObstacleGenerator _obstacleGenerator;
        public PlayerActionInfo.PlayerAction actionTheme;

        private void Update()
        {
            if (transform.position.z < _destroyPosZ)
            {
                if (_obstacleGenerator != null)
                {
                    _obstacleGenerator.CheckGenerateOrStartSprint(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }                
            }
        }
    }
}

