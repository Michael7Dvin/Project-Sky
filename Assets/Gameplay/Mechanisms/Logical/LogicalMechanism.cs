using UniRx;
using UnityEngine;

public abstract class LogicalMechanism : MonoBehaviour
{
    public abstract IReadOnlyReactiveProperty<bool> Output { get; }
}
