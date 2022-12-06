using UnityEngine;
using UniRx;

[RequireComponent(typeof(DisappearingPlatform))]
public class DisappearingPlatformReactivator : MonoBehaviour
{
    [SerializeField] private LogicalMechanism[] _appearingInputs;
    [SerializeField] private LogicalMechanism[] _disappearingInputs;

    private DisappearingPlatform _platform;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _platform = GetComponent<DisappearingPlatform>();
    }

    private void OnEnable()
    {
        foreach (LogicalMechanism input in _appearingInputs)
        {
            input
                .ReadOnlyOutput
                .Skip(1)
                .Where(value => value == true)
                .Subscribe(value => _platform.Appear())
                .AddTo(_disposable);
        }

        foreach (LogicalMechanism input in _disappearingInputs)
        {
            input
                .ReadOnlyOutput
                .Skip(1)
                .Where(value => value == false)
                .Subscribe(value => _platform.Disappear())
                .AddTo(_disposable);
        }
    }

    private void OnDisable() => _disposable.Clear();
}
