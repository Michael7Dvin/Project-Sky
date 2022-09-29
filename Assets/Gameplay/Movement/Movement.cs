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
                case MovementState.Running:
                    return InputMagnitude * _runSpeed;
                case MovementState.Sprinting:
                    return InputMagnitude * _sprintSpeed;
                case MovementState.Sneaking:
                    return InputMagnitude * _sneakSpeed;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }

    [SerializeField][Range(0, 99)] private float _runSpeed;
    [SerializeField][Range(0, 99)] private float _sprintSpeed;
    [SerializeField][Range(0, 99)] private float _sneakSpeed;

    [SerializeField] private float _rotationSpeed;

    private float _inputMagnitude;
    private float _movementVelocityMagnitudeFraction;

    private CharacterController _characterController;
    private CompositeDisposable _sprintPossibilityDisposable = new CompositeDisposable();

    public IReadOnlyReactiveProperty<MovementState> State => _state;
    public float MovementVelocityMagnitudeFraction => _movementVelocityMagnitudeFraction;
    public float RunSpeed => _runSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float SneakSpeed => _sneakSpeed;

    protected Vector3 MovementDirection { get; set; }
    public float InputMagnitude
    {
        get { return _inputMagnitude; }

        protected set
        {
            if(value < 0f)
            {
                Debug.LogException(new ArgumentException());
                _inputMagnitude = 0;
            }
            else
            {
                _inputMagnitude = value;
            }
        }
    }    

    protected virtual void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        _sprintPossibilityDisposable.Clear();
    }

    protected virtual void Update()
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);

        MoveHorizontal();

        if(float.IsNaN(_characterController.velocity.magnitude / MoveSpeed))
        {
            _movementVelocityMagnitudeFraction = 0f;
        }
        else
        {
            _movementVelocityMagnitudeFraction = Mathf.Clamp01(_characterController.velocity.magnitude / MoveSpeed);
        }                 
    }
    
    protected void SwitchState(MovementState state)
    {
        _state.Value = state;
    }

    private void MoveHorizontal()
    {
        if (MovementDirection != Vector3.zero)
        {
            if (_characterController.isGrounded == true)
            {
                MovementDirection.Normalize();

                Vector3 velocity = MoveSpeed * MovementDirection * _inputMagnitude;
                _characterController.Move(velocity * Time.deltaTime);

                Quaternion rotation = Quaternion.LookRotation(MovementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}
