using UnityEngine;

public class KeyLock : BaseLock
{
    [SerializeField] private bool _isLocked = true;
    [SerializeField] protected KeyType _unlockingKeyType;

    public override bool IsLocked => _isLocked;
    public KeyType UnlockingKeyType => _unlockingKeyType;
    
    public bool TryUnlock(Key key)
    {
        if (IsLocked == false)
        {
            return true;
        }

        if (key.KeyType == UnlockingKeyType)
        {
            _isLocked = false;
            key.SpendUse();
            return true;
        }

        return false;
    }

    public void Lock() => _isLocked = true;
}
