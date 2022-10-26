using UnityEngine;

public class ItemCollector : MonoBehaviour
{    
    [SerializeField] private ResourcesStorage _resourcesStorage;
    [SerializeField] private KeyHolder _keyHolder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PickUpableResource collectableResource))
        {
            _resourcesStorage.Add(collectableResource.Type, collectableResource.Amount);
            Destroy(collectableResource.gameObject);
        }

        if (other.TryGetComponent(out PickUpableKey pickUpableKey))
        {
            _keyHolder.AddKey(pickUpableKey.Key);
            Destroy(pickUpableKey.gameObject);
        }
    }
}
