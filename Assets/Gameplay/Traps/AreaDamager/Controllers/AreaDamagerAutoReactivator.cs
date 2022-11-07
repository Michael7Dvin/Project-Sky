using System.Collections;
using UnityEngine;

public class AreaDamagerAutoReactivator : BaseAreaDamagerReactivator
{
    [Range(0, float.MaxValue)]
    [SerializeField] private float _delay;
    private WaitForSeconds _waitForDelay;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _activatedTime;
    private WaitForSeconds _waitForActivatedTime;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _deactivatedTime;
    private WaitForSeconds _waitForDeactivatedTime;

    protected override void Awake()
    {
        base.Awake();

        _waitForDelay = new WaitForSeconds(_delay);
        _waitForActivatedTime = new WaitForSeconds(_activatedTime);
        _waitForDeactivatedTime = new WaitForSeconds(_deactivatedTime);

        StartCoroutine(Reactivating());
    }

    private IEnumerator Reactivating()
    {
        yield return _waitForDelay;

        while (true)
        {         
            _areaDamager.Activate();
            yield return _waitForActivatedTime;

            _areaDamager.Deactivate();
            yield return _waitForDeactivatedTime;
        }
    }
}
