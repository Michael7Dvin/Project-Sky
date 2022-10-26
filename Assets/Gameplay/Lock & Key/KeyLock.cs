public class KeyLock
{
    public KeyLock(bool isLocked, KeyType unlockingKeyType)
    {
        IsLocked = isLocked;
        UnlockingKeyType = unlockingKeyType;
    }

    public bool IsLocked { get; private set; }
    public KeyType UnlockingKeyType { get; private set; }

    public bool TryUnlock(Key key)
    {
        if (IsLocked == false)
        {
            return true;
        }

        if (key.KeyType == UnlockingKeyType)
        {
            IsLocked = false;
            key.SpendUse();
            return true;
        }

        return false;
    }
}
