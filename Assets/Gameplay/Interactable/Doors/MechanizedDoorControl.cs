using UnityEngine;
using UniRx;

[RequireComponent(typeof(BaseDoor))]
public class MechanizedDoorControl : MonoBehaviour
{
    private BaseDoor _door;
    [SerializeField] private LogicalMechanism _logicalMechanism;

    [SerializeField] private bool _isSingleUse;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void OnValidate()
    {
        if (_logicalMechanism == null)
        {
            Debug.LogError($"{gameObject} LogicalMechanism is null");
        }
    }

    private void Awake() => _door = GetComponent<BaseDoor>();

    private void OnEnable()
    {
        _logicalMechanism
            .Output
            .Subscribe(value => OnOutputValueChanged(value))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
    
    private void OnOutputValueChanged(bool value)
    {
        if (value == true)
        {
            _door.Open();
        }
        else
        {
            _door.Close();
        }

        if (_isSingleUse == true)
        {
            _disposable.Clear();
        }
    }
}
