using UnityEngine;

public class ResourcesCollector : MonoBehaviour
{    
    [SerializeField] private ResourcesData _resourcesData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectableResource collectableResource))
        {
            _resourcesData.Add(collectableResource.Type, collectableResource.Amount);
            Destroy(collectableResource.gameObject);
        }
    }
}
