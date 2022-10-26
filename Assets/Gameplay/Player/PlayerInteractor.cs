using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class PlayerInteractor : MonoBehaviour
{
    private readonly Dictionary<IInteractable, GameObject> _inRangeInteractables = new Dictionary<IInteractable, GameObject>();
    private readonly ReactiveProperty<KeyValuePair<IInteractable, GameObject>> _currentInteractable = new ReactiveProperty<KeyValuePair<IInteractable, GameObject>>(default);

    private PlayerInput _input;
    [SerializeField] private KeyHolder _keyHolder;

    private bool IsCurrentInteractableDefault => _currentInteractable.Value.Equals(default(KeyValuePair<IInteractable, GameObject>));

    private void OnEnable()
    {
        _input = new PlayerInput();
        _input.Enable();

        _input.Interaction.Interact.started += context => Interact();
    }

    private void OnDisable() => _input.Disable();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _inRangeInteractables.Add(interactable, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (_inRangeInteractables.TryGetValue(interactable, out _))
            {
                _inRangeInteractables.Remove(interactable);
            }
        }
    }

    private void Update()
    {
        CalculateCurrentInteractable();
    }

    private void CalculateCurrentInteractable()
    {
        switch (_inRangeInteractables.Count)
        {
            case 0:
                _currentInteractable.Value = default;
                return;
            case 1:
                if (_inRangeInteractables.FirstOrDefault().Key.IsActive == true)
                {
                    _currentInteractable.Value = _inRangeInteractables.FirstOrDefault();
                }                     
                return;
        }

        Dictionary<IInteractable, GameObject> activeInteractables =
            _inRangeInteractables
            .Where(interactable => interactable.Key.IsActive == true)
            .ToDictionary(_ => _.Key, _ => _.Value);

        switch (activeInteractables.Count)
        {
            case 0:
                _currentInteractable.Value = default;
                return;
            case 1:
                _currentInteractable.Value = activeInteractables.FirstOrDefault();
                return;
        }

        Vector2 playerHorizontalPosition = new Vector2(transform.position.x, transform.position.z);

        KeyValuePair <IInteractable, GameObject> nearest = activeInteractables
            .OrderBy(interactable => GetHorizontalDistanceToPlayer(interactable.Value.transform.position))
            .First();

        _currentInteractable.Value = nearest;

        float GetHorizontalDistanceToPlayer(Vector3 position)
        {
            Vector2 horizontalPosition = new Vector2(position.x, position.z);
            return Vector2.Distance(playerHorizontalPosition, horizontalPosition);
        }
    }

    private void Interact()
    {
        if (IsCurrentInteractableDefault == false)
        {
            if (_currentInteractable.Value.Value.TryGetComponent(out ILockableWithKey lockable))
            {
                TryOpenLockable(lockable);
            }
            
            _currentInteractable.Value.Key.Interact();            
        }

        bool TryOpenLockable(ILockableWithKey keyLock)
        {
            if (keyLock.IsLocked == true)
            {
                foreach (Key key in _keyHolder.Keys)
                {
                    if (keyLock.TryUnlock(key) == true)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
