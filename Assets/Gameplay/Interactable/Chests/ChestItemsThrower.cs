 using UnityEngine;
using UniRx;
using DG.Tweening;

[RequireComponent(typeof(Chest))]
public class ChestItemsThrower : MonoBehaviour
{
    [SerializeField] private Transform[] _throwPositions;
    private int _currentThrowPositionId = 0;

    [SerializeField] private float _throwPower;
    [SerializeField] private float _throwSpeed;

    private Chest _chest;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _chest = GetComponent<Chest>();

        if (_throwPositions.Length == 0)
        {
            Debug.LogError("Throw positions is empty");
        }
    }

    private void OnEnable()
    {
        _chest
            .ItemSpawned
            .Subscribe(item => Throw(item))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();

    private void Throw(PickUpable pickUpable)
    {
        if (_currentThrowPositionId == _throwPositions.Length)
        {
            _currentThrowPositionId = 0;
        }

        GetThrowTween(pickUpable.transform, _throwPositions[_currentThrowPositionId].position);
        _currentThrowPositionId++;
    }

    private Tween GetThrowTween(Transform itemTransform, Vector3 throwPosition)
    {
        int throwCount = 1;

        return itemTransform
            .DOJump(throwPosition, _throwPower, throwCount, _throwSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Normal)
            .SetLink(itemTransform.gameObject);
    }
}
