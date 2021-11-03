using UnityEngine;

namespace XRLab.VRoem.Core
{
    public abstract class Pickup : MonoBehaviour 
    {
        public enum PickupType
        {
            Repair,
            Speedboost
        }

        [SerializeField] private PickupType _pickupType;
        
        private CollectableCounter _collectableCounter; 
        private void Start()
        {
            _collectableCounter = FindObjectOfType<CollectableCounter>();
        }
    
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag(Tags.PLAYER)) {
                OnPickupTriggered();
                RemovePickup();
            }
        }

        protected void AddPickupCount()
        {
            _collectableCounter.AddPickupCount(_pickupType);
        }
        protected abstract void OnPickupTriggered();

        protected virtual void RemovePickup()
        {
            Destroy(this);
        }
    }
}
