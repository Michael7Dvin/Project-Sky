using UnityEngine;

[RequireComponent(typeof(BaseAreaDamager))]
public abstract class BaseAreaDamagerReactivator : MonoBehaviour
{
    protected BaseAreaDamager _areaDamager;

    protected virtual void Awake()
    {
        _areaDamager = GetComponent<BaseAreaDamager>();
    }
}
