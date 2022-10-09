using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    private Rigidbody _rigidbody;
    private readonly ReactiveProperty<bool> _isGrounded = new ReactiveProperty<bool>();

    public IReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
        Debug.Log(IsGrounded);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _groundLayerMask) != 0)
        {
            _isGrounded.Value = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _groundLayerMask) != 0)
        {
            _isGrounded.Value = false;
        }
    }
}
