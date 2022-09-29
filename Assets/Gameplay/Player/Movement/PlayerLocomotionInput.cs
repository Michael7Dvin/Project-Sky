using UnityEngine;

public class PlayerLocomotionInput : LocomotionInput
{
    [SerializeField] Transform _camera;

    private Vector3 _notAllignToCameraInputDirection;
    private PlayerInput _input;

    private void Awake()
    {       
        _input = new PlayerInput();

        _input.Movement.HorizontalMovement.started += OnHorizontalLocomotionInput;
        _input.Movement.HorizontalMovement.performed += OnHorizontalLocomotionInput;
        _input.Movement.HorizontalMovement.canceled += OnHorizontalLocomotionInput;

        _input.Movement.Sprint.started += context => _locomotionInputAction.Value = (LocomotionState.Sprinting, true);
        _input.Movement.Sprint.canceled += context => _locomotionInputAction.Value = (LocomotionState.Sprinting, false);

        _input.Movement.Sneak.started += context => _locomotionInputAction.Value = (LocomotionState.Sneaking, true);
        _input.Movement.Sneak.canceled += context => _locomotionInputAction.Value = (LocomotionState.Sneaking, false);
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Update()
    {
        InputDirection = Quaternion.AngleAxis(_camera.rotation.eulerAngles.y, Vector3.up) * _notAllignToCameraInputDirection;
    }

    private void OnHorizontalLocomotionInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _notAllignToCameraInputDirection = context.ReadValue<Vector2>();
        _notAllignToCameraInputDirection = new Vector3(_notAllignToCameraInputDirection.x, 0, _notAllignToCameraInputDirection.y);
    }
}
