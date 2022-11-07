using System;
using UnityEngine;
using UniRx;

public class Health : MonoBehaviour
{
    [SerializeField] private FloatReactiveProperty _amount = new FloatReactiveProperty();
    private readonly ReactiveCommand _died = new ReactiveCommand();

    public ReactiveProperty<float> Amount => _amount;
    public IObservable<Unit> Died => _died;

    private void OnValidate()
    {
        if (_amount.Value <= 0)
        {
            Debug.LogWarning($"{gameObject} Health amount must be greater than zero");
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError($"{gameObject} damage cannot be negative");
            return;
        }

        float damaged = _amount.Value - damage;
        
        if (damaged <= 0)
        {
            _amount.Value = 0;
            _died.Execute();
        }
        else
        {
            _amount.Value = damaged;
        }

        Debug.Log(_amount.Value);
    }
}
