using UniRx;

public class Key
{
    public readonly KeyType KeyType;
    public readonly uint _usesInitialAmount;

    private readonly ReactiveProperty<uint> _remainingUses = new ReactiveProperty<uint>();

    public Key(KeyType keyType, uint usesInitialAmount)
    {
        KeyType = keyType;
        _usesInitialAmount = usesInitialAmount;

        _remainingUses.Value = usesInitialAmount;
    }

    public IReadOnlyReactiveProperty<uint> RemainingUses => _remainingUses;

    public void SpendUse()
    {
        if (_remainingUses.Value > 0)
        {
            _remainingUses.Value -= 1;
        }
    }
}
