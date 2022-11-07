using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FlickeringPlatform : MonoBehaviour
{
    [Range(0, float.MaxValue)]
    [SerializeField] private float _delay;
    private WaitForSeconds _waitForDelay;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _appearedTime;
    private WaitForSeconds _waitForAppearedTime;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _disappearedTime;
    private WaitForSeconds _waitForDisappearedTime;

    private Collider _collider;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _waitForAppearedTime = new WaitForSeconds(_appearedTime);
        _waitForDisappearedTime = new WaitForSeconds(_disappearedTime);
        _waitForDelay = new WaitForSeconds(_delay);

        StartCoroutine(Flickering());
    }

    private IEnumerator Flickering()
    {
        yield return _waitForDelay;
        
        while (true)
        {
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            yield return _waitForDisappearedTime;

            _collider.enabled = true;
            _meshRenderer.enabled = true;
            yield return _waitForAppearedTime;
        }
    }
}
