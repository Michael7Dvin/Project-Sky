using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PressurePlate : LogicalMechanism
{
    [SerializeField] private Collider _pressingAreaTrigger;

    private readonly ReactiveCollection<LocomotionComposition> _pressingObjects = new ReactiveCollection<LocomotionComposition>();

    private void OnValidate()
    {
        if (_pressingAreaTrigger == null)
        {
            Debug.LogError($"{gameObject} Pressing Area Trigger cannot be null");
        }
        else
        {
            _pressingAreaTrigger.isTrigger = true;
        }
    }

    private void OnEnable()
    {
        foreach (LogicalMechanism input in _inputs)
        {
            input
                .Output
                .Skip(1)
                .Subscribe(value => _output.Value = value)
                .AddTo(Disposable);
        }

        SubscribeOnPressingAreaTrigger();
        SubscribeOnPressingObjectsChanging();

        void SubscribeOnPressingAreaTrigger()
        {
            _pressingAreaTrigger
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.TryGetComponent(out LocomotionComposition locomotionComposition))
                    {
                        _pressingObjects.Add(locomotionComposition);
                    }
                })
                .AddTo(Disposable);

            _pressingAreaTrigger
                .OnTriggerExitAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.TryGetComponent(out LocomotionComposition locomotionComposition))
                    {
                        _pressingObjects.Remove(locomotionComposition);
                    }
                })
                .AddTo(Disposable);
        }            
        void SubscribeOnPressingObjectsChanging()
        {
            _pressingObjects
                .ObserveAdd()
                .Subscribe(_ => OnPressingObjectsChanged())
                .AddTo(Disposable);

            _pressingObjects
                .ObserveRemove()
                .Subscribe(_ => OnPressingObjectsChanged())
                .AddTo(Disposable);

            _pressingObjects
                .ObserveReplace()
                .Subscribe(_ => OnPressingObjectsChanged())
                .AddTo(Disposable);
        }
        void OnPressingObjectsChanged()
        {
            if (_pressingObjects.Count == 0)
            {
                _output.Value = false;
            }
            else
            {
                _output.Value = true;
            }
        }
    }

    private void Start() => SetInitialOutputValue(OrInputsValue);
}
