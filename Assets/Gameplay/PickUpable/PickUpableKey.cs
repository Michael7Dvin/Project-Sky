using UnityEngine;

public class PickUpableKey : PickUpable
{
    [SerializeField] private KeyType _type;
    [SerializeField] private uint _usesAmount;

    private Key _key;

    private void Awake()
    {
        _key = new Key(_type, _usesAmount);

        if (_usesAmount == 0)
        {
            Debug.LogWarning($"{gameObject} Key have zero uses amount");
        }
    }

    public Key Key => _key;
}
