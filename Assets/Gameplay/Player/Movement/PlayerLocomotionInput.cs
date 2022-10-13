using UnityEngine;

public class PlayerLocomotionInput : LocomotionInput
{
    [SerializeField] Transform _camera;

    private Vector3 _notAlignedToCameraInputDirection;
    private PlayerInput _input;

    private void Awake()
    {       
        _input = new PlayerInput();

        _input.Movement.HorizontalMovement.started += OnHorizontalInput;
        _input.Movement.HorizontalMovement.performed += OnHorizontalInput;
        _input.Movement.HorizontalMovement.canceled += OnHorizontalInput;

        _input.Movement.VerticalMovement.started += OnVerticalInput;
        _input.Movement.VerticalMovement.performed += OnVerticalInput;
        _input.Movement.VerticalMovement.canceled += OnVerticalInput;

        _input.Movement.Jump.started += conext => _currentLocomotionType.SetValueAndForceNotify(LocomotionType.Jump);
        _input.Movement.Fly.started += context => _currentLocomotionType.SetValueAndForceNotify(LocomotionType.Fly);
        _input.Movement.Dodge.started += context => _currentLocomotionType.SetValueAndForceNotify(LocomotionType.Dodge);

        _input.Movement.Sprint.started += context => _locomotionMoveSpeedTypeAction.Value = (LocomotionMoveSpeedType.Sprint, true);
        _input.Movement.Sprint.canceled += context => { _locomotionMoveSpeedTypeAction.Value = (LocomotionMoveSpeedType.Sprint, false); Debug.Log(1); };

        _input.Movement.Sneak.started += context => _locomotionMoveSpeedTypeAction.Value = (LocomotionMoveSpeedType.Slow, true);
        _input.Movement.Sneak.canceled += context => _locomotionMoveSpeedTypeAction.Value = (LocomotionMoveSpeedType.Slow, false);
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Update()
    {
        Direction = Quaternion.AngleAxis(_camera.rotation.eulerAngles.y, Vector3.up) * _notAlignedToCameraInputDirection;
    }

    private void OnHorizontalInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _notAlignedToCameraInputDirection = context.ReadValue<Vector2>();
        _notAlignedToCameraInputDirection = new Vector3(_notAlignedToCameraInputDirection.x, 0, _notAlignedToCameraInputDirection.y);
    }

    private void OnVerticalInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _notAlignedToCameraInputDirection.y = context.ReadValue<float>();
    }
}
