using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FlickeringPlatform : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _delay;
    [Range(0, float.MaxValue)]
    [SerializeField] private float _appearedTime;
    [Range(0, float.MaxValue)]
    [SerializeField] private float _disappearedTime;

    private WaitForSeconds _waitDelay;
    private WaitForSeconds _waitAppearedTime;
    private WaitForSeconds _waitDisappearedTime;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _waitAppearedTime = new WaitForSeconds(_appearedTime);
        _waitDisappearedTime = new WaitForSeconds(_disappearedTime);
        _waitDelay = new WaitForSeconds(_delay);

        StartCoroutine(StartFlickering());
    }

    private IEnumerator StartFlickering()
    {
        yield return _waitDelay;
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;

        yield return _waitAppearedTime;
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;

        yield return _waitDisappearedTime;
        StartCoroutine(Appear());
    }
}
