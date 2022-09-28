using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
public abstract class Movement : MonoBehaviour
{
    private readonly ReactiveProperty<MovementState> _state = new ReactiveProperty<MovementState>();
    private float MoveSpeed
    {
        get
        {
            switch (_state.Value)
            {
                case MovementState.Walking:
                    return MovementMagnitude * _walkSpeed;
                case MovementState.Running:
                    return MovementMagnitude * _runSpeed;
                case MovementState.Sneaking:
                    return MovementMagnitude * _sneakSpeed;
                case MovementState.Jumping:
                    return MovementMagnitude * _jumpHorizontalMoveSpeed;
                case MovementState.Falling:
                    return 0;
                case MovementState.Sitting:
                    return 0;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }
    [SerializeField][Range(0, 99)] private float _walkSpeed;
    [SerializeField][Range(0, 99)] private float _runSpeed;
    [SerializeField][Range(0, 99)] private float _sneakSpeed;
    [SerializeField][Range(0, 99)] private float _jumpHorizontalMoveSpeed;

    [SerializeField] private float _rotationSpeed;

    private float _inputMovementMagnitude;

    private CharacterController _characterController;

    protected Vector3 MovementDirection { get; set; }
    protected float MovementMagnitude
    {
        get { return _inputMovementMagnitude; }

        set
        {
            if(value < 0f)
            {
                Debug.LogException(new ArgumentException());
                _inputMovementMagnitude = 0;
            }
            else
            {
                _inputMovementMagnitude = value;
            }
        }
    }    

    protected virtual void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);
        MoveHorizontal();
    }
    
    protected void SwitchState(MovementState state)
    {
        _state.Value = state;
    }

    private void MoveHorizontal()
    {
        if (MovementDirection != Vector3.zero)
        {
            if(_characterController.isGrounded == true)
            {
                MovementDirection.Normalize();

                Vector3 velocity = MoveSpeed * MovementDirection;
                _characterController.Move(velocity * Time.deltaTime);

                Quaternion rotation = Quaternion.LookRotation(MovementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}
