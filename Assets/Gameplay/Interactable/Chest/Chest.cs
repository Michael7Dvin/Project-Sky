using System.Collections;
using UnityEngine;
using UniRx;
using System;

public class Chest : MonoBehaviour, IInteractable, ILockableWithKey
{
    [SerializeField] private bool _isActive;

    [SerializeField] private PickUpable[] _containedItems;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private bool _isLockedInitialValue;
    [SerializeField] private KeyType _unlockingKeyType;
    private KeyLock _keyLock;

    private readonly WaitForSeconds SpawningStartDelay = new WaitForSeconds(3f);
    private readonly WaitForSeconds SpawningInterval = new WaitForSeconds(0.5f);

    private readonly ReactiveCommand _chestOpened = new ReactiveCommand();
    private readonly ReactiveCommand<PickUpable> _itemSpawned = new ReactiveCommand<PickUpable>();

    public bool IsActive => _isActive;

    public IObservable<Unit> ChestOpened => _chestOpened;
    public IObservable<PickUpable> ItemSpawned => _itemSpawned;

    public bool IsLocked => _keyLock.IsLocked;

    private void Awake()
    {
        _keyLock = new KeyLock(_isLockedInitialValue, _unlockingKeyType);
    }

    public bool TryUnlock(Key key)
    {
        return _keyLock.TryUnlock(key);
    }

    public void Interact()
    {    
        if (_isActive == true)
        {
            if (IsLocked == false)
            {
                _chestOpened.Execute();
                StartCoroutine(SpawnItems());
            }
        }
    }

    private IEnumerator SpawnItems()
    {
        _isActive = false;

        yield return SpawningStartDelay;

        foreach (PickUpable item in _containedItems)
        {
            PickUpable spawnedItem = Instantiate(item, _spawnPoint.position, item.transform.rotation);
            _itemSpawned.Execute(spawnedItem);

            yield return SpawningInterval;
        }
    }

}
