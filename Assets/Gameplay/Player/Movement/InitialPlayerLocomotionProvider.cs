using UnityEngine;

[RequireComponent(typeof(LocomotionComposition))]
public class InitialPlayerLocomotionProvider : MonoBehaviour
{
    [SerializeField] private float _groundRunSpeed;
    [SerializeField] private float _groundSprintSpeed;
    [SerializeField] private float _groundSneakSpeed;

    [SerializeField] private float _fallVerticalSpeed;
    [SerializeField] private float _fallHorizontalSpeed;
    [SerializeField] private float _fallRotationSpeed;

    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _jumpNormalHorizontalSpeed;
    [SerializeField] private float _jumpSprintHorizontalSpeed;
  

    private void Awake()
    {
        LocomotionComposition locomotionComposition = GetComponent<LocomotionComposition>();

        DefaultGroundLocomotion _defaultGroundLocomotion = new DefaultGroundLocomotion(_groundRunSpeed, _groundSprintSpeed, _groundSneakSpeed);
        DefaultFallLocomotion _defaultFallLocomotion = new DefaultFallLocomotion(_fallVerticalSpeed, _fallHorizontalSpeed);
        DefaultJumpLocomotion _defaultJumpLocomotion = new DefaultJumpLocomotion(_jumpSpeed, _jumpNormalHorizontalSpeed, _jumpSprintHorizontalSpeed);
        NoneFlyLocomotion _noneFlyLocomotion = new NoneFlyLocomotion();

        locomotionComposition.ChangeLocomotion(_defaultGroundLocomotion);
        locomotionComposition.ChangeLocomotion(_defaultFallLocomotion);
        locomotionComposition.ChangeLocomotion(_defaultJumpLocomotion);
        locomotionComposition.ChangeLocomotion(_noneFlyLocomotion);

        //FreeFlyLocomotion _freeFlyLocomotion = new FreeFlyLocomotion(3f, 6f, 3f, 6f);
        //locomotionComposition.ChangeLocomotion(_freeFlyLocomotion);

        //GlideFlyLocomotion _glideFlyLocomotion = new GlideFlyLocomotion(-3f, -5f, 2f, 6f);
        //locomotionComposition.ChangeLocomotion(_glideFlyLocomotion);

        //MultiJumpLocomotion _multiJumpLocomotion = new MultiJumpLocomotion(_jumpSpeed, _jumpNormalHorizontalSpeed, _jumpSprintHorizontalSpeed, 1);
        //locomotionComposition.ChangeLocomotion(_multiJumpLocomotion);

        Destroy(this);
    }
}
