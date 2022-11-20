using UnityEngine;
using UniRx;

[RequireComponent(typeof(BaseAreaDamager))]
public class AreaDamagerReactivator : MonoBehaviour
{
    [SerializeField] private LogicalMechanism[] _activatingInputs;
    [SerializeField] private LogicalMechanism[] _deactivatingInputs;

    private BaseAreaDamager _areaDamager;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _areaDamager = GetComponent<BaseAreaDamager>();       
    }

    private void OnEnable()
    {
        foreach (LogicalMechanism input in _activatingInputs)
        {
            input
                .ReadOnlyOutput
                .Skip(1)
                .Where(value => value == true)
                .Subscribe(value => _areaDamager.Activate())
                .AddTo(_disposable);
        }
       
        foreach (LogicalMechanism input in _deactivatingInputs)
        {
            input
                .ReadOnlyOutput
                .Skip(1)
                .Where(value => value == false)
                .Subscribe(value => _areaDamager.Deactivate())
                .AddTo(_disposable);
        }
    }

    private void OnDisable() => _disposable.Clear();
}
