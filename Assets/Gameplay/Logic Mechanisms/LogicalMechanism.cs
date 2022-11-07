using UniRx;
using UnityEngine;

public abstract class LogicalMechanism : MonoBehaviour
{
    protected readonly ReactiveProperty<bool> _output = new ReactiveProperty<bool>();
    protected readonly CompositeDisposable _disposable = new CompositeDisposable();

    public IReadOnlyReactiveProperty<bool> Output => _output;

    private void OnDisable() => _disposable.Clear();
}
