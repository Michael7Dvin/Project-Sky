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

    private void Awake()
    {
        LocomotionComposition locomotionComposition = GetComponent<LocomotionComposition>();

        DefaultGroundLocomotion _defaultGroundLocomotion = new DefaultGroundLocomotion(_groundRunSpeed, _groundSprintSpeed, _groundSneakSpeed, _groundRotationSpeed);
        DefaultFallLocomotion _defaultFallLocomotion = new DefaultFallLocomotion(_fallVerticalSpeed, _fallHorizontalSpeed, _fallRotationSpeed);

        locomotionComposition.ChangeGroundLocomotion(_defaultGroundLocomotion);
        locomotionComposition.ChangeFallLocomotion(_defaultFallLocomotion);

        //GlideFallLocomotion _glideFallLocomotion = new GlideFallLocomotion(-4f, 3f, 6f, 1.5f, 1.5f, _fallRotationSpeed);
        //locomotionComposition.ChangeFallLocomotion(_glideFallLocomotion);

        Destroy(this);
    }
}
