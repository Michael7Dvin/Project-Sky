using UnityEngine;
using UniRx;

public class ResourcesStorage : MonoBehaviour
{
    private readonly ReactiveDictionary<ResourceType, uint> _resources = new ReactiveDictionary<ResourceType, uint>();
    public IReadOnlyReactiveDictionary<ResourceType, uint> Resources => _resources;

    public void Set(ResourceType type, uint amount) => _resources[type] = amount;

    public void Add(ResourceType type, uint amount)
    {
        if (_resources.TryGetValue(type, out uint value) == false)
        {
            _resources[type] = amount;
        }
        else
        {
            _resources[type] = value + amount;
        }
    }

    public bool TrySpend(ResourceType type, uint amount)
    {
        if (_resources.TryGetValue(type, out uint value))
        {
            if (value > amount)
            {
                _resources[type] = value - amount;
            }
        }
        return false;
    }
}
