using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Detector : LogicalMechanism
{
    [SerializeField] private Collider _detectingAreaTrigger;

    private readonly ReactiveCollection<LocomotionComposition> _detectedObjects = new ReactiveCollection<LocomotionComposition>();

    private void OnValidate()
    {
        if (_detectingAreaTrigger == null)
        {
            Debug.LogError($"{gameObject} Pressing Area Trigger cannot be null");
        }
        else
        {
            _detectingAreaTrigger.isTrigger = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        SubscribeOnDetectingAreaTrigger();
        SubscribeOnDetectedObjectsChanging();

        void SubscribeOnDetectingAreaTrigger()
        {
            _detectingAreaTrigger
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.TryGetComponent(out LocomotionComposition locomotionComposition))
                    {
                        _detectedObjects.Add(locomotionComposition);
                    }
                })
                .AddTo(Disposable);

            _detectingAreaTrigger
                .OnTriggerExitAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.TryGetComponent(out LocomotionComposition locomotionComposition))
                    {
                        _detectedObjects.Remove(locomotionComposition);
                    }
                })
                .AddTo(Disposable);
        }            
        void SubscribeOnDetectedObjectsChanging()
        {
            _detectedObjects
                .ObserveAdd()
                .Subscribe(_ => OnDetectedObjectsChanged())
                .AddTo(Disposable);

            _detectedObjects
                .ObserveRemove()
                .Subscribe(_ => OnDetectedObjectsChanged())
                .AddTo(Disposable);

            _detectedObjects
                .ObserveReplace()
                .Subscribe(_ => OnDetectedObjectsChanged())
                .AddTo(Disposable);
        }
        void OnDetectedObjectsChanged()
        {
            if (_detectedObjects.Count == 0)
            {
                Output.Value = false;
            }
            else
            {
                Output.Value = true;                
            }
        }
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Subscribe(value => Output.Value = value)
            .AddTo(Disposable);
    }
}
