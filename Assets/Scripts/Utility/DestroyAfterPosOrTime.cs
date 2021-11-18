using UnityEngine;

namespace XRLab.VRoem.Utility
{
    public class DestroyAfterPosOrTime : MonoBehaviour
    {
        [SerializeField] private float _positionToDestroyZ = 2;

        // spawntimer
        [SerializeField] private float secondsBetweenDestroy;
        private float elapsedTime = 0.0f;

        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (transform.position.z < _positionToDestroyZ)
            {
                Destroy(gameObject);
            }
           
            else if (elapsedTime > secondsBetweenDestroy && secondsBetweenDestroy != 0)
            {
                Destroy(gameObject);
                elapsedTime = 0;
            }

        }
    }
}

