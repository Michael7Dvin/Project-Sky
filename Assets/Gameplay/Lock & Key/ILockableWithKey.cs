public interface ILockableWithKey 
{
    bool IsLocked { get; }

    bool TryUnlock(Key key);
}
