using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    [SerializeField] Transform _camera;
    private PlayerInput _input;
    private Vector3 _inputMovementDirection;

    protected override void Awake()
    {
        base.Awake();

        _input = new PlayerInput();

        _input.Movement.HorizontalMovement.started += OnHorizontalMovementInput;
        _input.Movement.HorizontalMovement.performed += OnHorizontalMovementInput;
        _input.Movement.HorizontalMovement.canceled += OnHorizontalMovementInput;

        _input.Movement.Run.started += context => SwitchState(MovementState.Running);
        _input.Movement.Run.canceled += context => SwitchState(MovementState.Walking);
    }

    private void OnEnable()
    {
        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }

    protected override void Update()
    {
        MovementDirection = Quaternion.AngleAxis(_camera.rotation.eulerAngles.y, Vector3.up) * _inputMovementDirection;
        base.Update();
    }

    private void OnHorizontalMovementInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _inputMovementDirection = context.ReadValue<Vector2>();
        _inputMovementDirection = new Vector3(_inputMovementDirection.x, 0, _inputMovementDirection.y);
       
        MovementMagnitude = Mathf.Clamp01(_inputMovementDirection.magnitude);

        if (MovementMagnitude > 0.5)
        {
            MovementMagnitude = 1;
        }
    }
}
