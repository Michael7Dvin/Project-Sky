using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class CollapsingPlatform : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _collapsingTime;

    [SerializeField] private bool _isRespawning;
    [Range(0, float.MaxValue)]
    [SerializeField] private float _respawnTime;

    private WaitForSeconds _wait—ollapsingTime;
    private WaitForSeconds _waitRespawnTime;

    private bool _isCollapsed;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _wait—ollapsingTime = new WaitForSeconds(_collapsingTime);
        _waitRespawnTime = new WaitForSeconds(_respawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollapsed == false)
        {
            if (other.TryGetComponent(out LocomotionComposition _))
            {
                StartCoroutine(Collapse());
            }
        }
    }

    private IEnumerator Collapse()
    {
        yield return _wait—ollapsingTime;

        _collider.enabled = false;
        _meshRenderer.enabled = false;
        _isCollapsed = true;

        if (_isRespawning == true)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return _waitRespawnTime;

        _collider.enabled = true;
        _meshRenderer.enabled = true;
        _isCollapsed = false;
    }
}
