using UnityEngine;
using UniRx;
using DG.Tweening;
using System;

[RequireComponent(typeof(Collider))]
public abstract class MovingPlatform : MonoBehaviour
{
    [Range(0f, float.MaxValue)]
    [SerializeField] private float _speed;

    private readonly ReactiveCommand<Transform> _movementCompleted = new ReactiveCommand<Transform>();
    public IObservable<Transform> MovementCompleted => _movementCompleted;

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
        transform.localScale = Vector3.one;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LocomotionComposition locomotionComposition))
        {
            locomotionComposition.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out LocomotionComposition locomotionComposition))
        {
            locomotionComposition.transform.SetParent(null);
        }
    }

    protected void Move(Transform movePoint)
    {
        DOTween.Kill(transform);

        transform
            .DOMove(movePoint.position, _speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed)
            .SetLink(gameObject)
            .OnComplete(() => _movementCompleted.Execute(movePoint));
    }
}
