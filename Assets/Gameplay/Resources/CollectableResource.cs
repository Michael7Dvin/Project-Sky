using UnityEngine;

public class CollectableResource : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private uint _amount;

    public ResourceType Type => _resourceType;
    public uint Amount => _amount;
}
