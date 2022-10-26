using UnityEngine;
using UniRx;

[RequireComponent(typeof(Chest), typeof(Animator))]
public class ChestAnimator : MonoBehaviour
{
    private readonly int _openTriggerParameterHash = Animator.StringToHash("Open");

    private Chest _chest;
    private Animator _animator;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _chest = GetComponent<Chest>();
        _animator = GetComponent<Animator>();   
    }

    private void OnEnable()
    {
        _chest
            .ChestOpened
            .Subscribe(_ => _animator.SetTrigger(_openTriggerParameterHash));
    }

    private void OnDisable() => _disposable.Clear();               
}
