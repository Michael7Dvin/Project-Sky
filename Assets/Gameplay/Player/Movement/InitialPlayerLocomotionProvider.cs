using UnityEngine;

[RequireComponent(typeof(LocomotionComposition))]
public class InitialPlayerLocomotionProvider : MonoBehaviour
{
    [SerializeField] private float _groundRunSpeed;
    [SerializeField] private float _groundSprintSpeed;
    [SerializeField] private float _groundSneakSpeed;
    [SerializeField] private float _groundRotationSpeed;

    [SerializeField] private float _fallVerticalSpeed;
    [SerializeField] private float _fallHorizontalSpeed;
    [SerializeField] private float _fallRotationSpeed;

    [SerializeField] private float _jumpSpeed;

    private void Awake()
    {
        LocomotionComposition locomotionComposition = GetComponent<LocomotionComposition>();

        DefaultGroundLocomotion _defaultGroundLocomotion = new DefaultGroundLocomotion(_groundRunSpeed, _groundSprintSpeed, _groundSneakSpeed, _groundRotationSpeed);
        DefaultFallLocomotion _defaultFallLocomotion = new DefaultFallLocomotion(_fallVerticalSpeed, _fallHorizontalSpeed, _fallRotationSpeed);
        DefaultJumpLocomotion _defaultJumpLocomotion = new DefaultJumpLocomotion(_jumpSpeed, 3f);

        locomotionComposition.ChangeGroundLocomotion(_defaultGroundLocomotion);
        locomotionComposition.ChangeFallLocomotion(_defaultFallLocomotion);
        locomotionComposition.ChangeJumpLocomotion(_defaultJumpLocomotion);


        //GlideFallLocomotion _glideFallLocomotion = new GlideFallLocomotion(-4f, -6f, 4f, 9f, -9.8f, 3f, _fallRotationSpeed);
        //locomotionComposition.ChangeFallLocomotion(_glideFallLocomotion);

        Destroy(this);
    }
}
