using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class DisappearingPlatform : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Appear()
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;
    }

    public void Disappear()
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;
    }
}
