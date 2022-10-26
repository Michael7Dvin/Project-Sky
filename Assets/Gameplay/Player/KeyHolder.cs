using UnityEngine;
using UniRx;

public class KeyHolder : MonoBehaviour
{
    private readonly ReactiveCollection<Key> _keys = new ReactiveCollection<Key>();
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    public IReadOnlyReactiveCollection<Key> Keys => _keys;

    private void OnDisable() => _disposable.Clear();

    public void AddKey(Key key)
    {
        _keys.Add(key);

        key
            .RemainingUses
            .Where(remainingUses => remainingUses == 0)
            .Subscribe(_ => RemoveKey(key))
            .AddTo(_disposable);
    }

    public void RemoveKey(Key key)
    {
        if (_keys.Contains(key) == true)
        {
            _keys.Remove(key);
        }
    }
}
