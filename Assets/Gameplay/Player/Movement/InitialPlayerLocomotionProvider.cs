using UnityEngine;

[RequireComponent(typeof(LocomotionComposition))]
public class InitialPlayerLocomotionProvider : MonoBehaviour
{
    [SerializeField] private float _groundJogSpeed;
    [SerializeField] private float _groundSprintSpeed;
    [SerializeField] private float _groundSneakSpeed;

    [SerializeField] private float _fallHorizontalSpeed;
    [SerializeField] private float _fallRotationSpeed;

    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _jumpNormalHorizontalSpeed;
    [SerializeField] private float _jumpSprintHorizontalSpeed;
    [SerializeField] private int _jumpAdditionalJumps;

    [SerializeField] private float _dodgeHorizontalSpeed;
    [SerializeField] private float _dodgeDuration;

    private void Awake()
    {
        LocomotionComposition locomotionComposition = GetComponent<LocomotionComposition>();

        DefaultGroundLocomotion _defaultGroundLocomotion = new DefaultGroundLocomotion(_groundJogSpeed, _groundSprintSpeed, _groundSneakSpeed);
        DefaultFallLocomotion _defaultFallLocomotion = new DefaultFallLocomotion(_fallHorizontalSpeed);
        MultiJumpLocomotion _defaultJumpLocomotion = new MultiJumpLocomotion(_jumpSpeed, _jumpNormalHorizontalSpeed, _jumpSprintHorizontalSpeed, _jumpAdditionalJumps);
        NoneFlyLocomotion _noneFlyLocomotion = new NoneFlyLocomotion();
        RollDodgeLocomotion _rollDodgeLocomotion = new RollDodgeLocomotion(_dodgeHorizontalSpeed, _dodgeDuration);
        
        locomotionComposition.ChangeLocomotion(_defaultGroundLocomotion);
        locomotionComposition.ChangeLocomotion(_defaultFallLocomotion);
        locomotionComposition.ChangeLocomotion(_defaultJumpLocomotion);
        locomotionComposition.ChangeLocomotion(_noneFlyLocomotion);
        locomotionComposition.ChangeLocomotion(_rollDodgeLocomotion);        

        //FreeFlyLocomotion _freeFlyLocomotion = new FreeFlyLocomotion(3f, 6f, 3f, 6f);
        //locomotionComposition.ChangeLocomotion(_freeFlyLocomotion);

        //GlideFlyLocomotion _glideFlyLocomotion = new GlideFlyLocomotion(-3f, -5f, 2f, 6f);
        //locomotionComposition.ChangeLocomotion(_glideFlyLocomotion);


        Destroy(this);
    }
}
