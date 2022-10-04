using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    private Rigidbody _rigidbody;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _groundLayerMask) != 0)
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _groundLayerMask) != 0)
        {
            IsGrounded = false;
        }
    }
}
