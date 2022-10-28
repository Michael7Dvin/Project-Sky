using System.Collections;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(BaseLock))]
public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isInteractionActive = true;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PickUpable[] _containedItems;

    private BaseLock _lock;

    private readonly WaitForSeconds SpawningStartDelay = new WaitForSeconds(3f);
    private readonly WaitForSeconds SpawningInterval = new WaitForSeconds(0.5f);

    private readonly ReactiveCommand _chestOpened = new ReactiveCommand();
    private readonly ReactiveCommand<PickUpable> _itemSpawned = new ReactiveCommand<PickUpable>();

    public bool IsInteractionActive => _isInteractionActive;

    public IObservable<Unit> ChestOpened => _chestOpened;
    public IObservable<PickUpable> ItemSpawned => _itemSpawned;

    private void Awake() => _lock = GetComponent<BaseLock>();

    public void Interact()
    {    
        if (_isInteractionActive == true)
        {
            if (_lock.IsLocked == false)
            {
                _chestOpened.Execute();
                StartCoroutine(SpawnItems());
            }
        }
    }

    private IEnumerator SpawnItems()
    {
        _isInteractionActive = false;

        yield return SpawningStartDelay;

        foreach (PickUpable item in _containedItems)
        {
            PickUpable spawnedItem = Instantiate(item, _spawnPoint.position, item.transform.rotation);
            _itemSpawned.Execute(spawnedItem);

            yield return SpawningInterval;
        }
    }

}
