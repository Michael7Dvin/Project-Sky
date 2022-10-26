using UnityEngine;

public class PickUpableResource : PickUpable
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private uint _amount;

    private void Awake()
    {
        if (_amount == 0)
        {
            Debug.LogWarning($"{gameObject} resource have zero value");
        }
    }

    public ResourceType Type => _resourceType;
    public uint Amount => _amount;
}
