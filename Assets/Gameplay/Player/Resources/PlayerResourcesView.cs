using UnityEngine;
using UniRx;

[RequireComponent(typeof(ResourcesStorage))]
public class PlayerResourcesView : MonoBehaviour
{
    private ResourcesStorage _resourcesData;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _resourcesData = GetComponent<ResourcesStorage>();
    }

    private void OnEnable()
    {
        _resourcesData
            .Resources
            .ObserveAdd()
            .Subscribe(_ => UpdateResource(_.Key, _.Value))
            .AddTo(_disposable);        
        
        _resourcesData
            .Resources
            .ObserveReplace()
            .Subscribe(_ => UpdateResource(_.Key, _.NewValue))
            .AddTo(_disposable);        
        
        _resourcesData
            .Resources
            .ObserveRemove()
            .Subscribe(_ => UpdateResource(_.Key, _.Value))
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }

    private void UpdateResource(ResourceType type, uint value)
    {
        Debug.Log($"Type: {type}, Value: {value}");
    }
}
