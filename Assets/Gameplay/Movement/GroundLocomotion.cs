using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
public class GroundLocomotion : MonoBehaviour
{
    private readonly ReactiveProperty<LocomotionState> _state = new ReactiveProperty<LocomotionState>();

    [SerializeField][Range(0, 99)] private float _runSpeed;
    [SerializeField][Range(0, 99)] private float _sprintSpeed;
    [SerializeField][Range(0, 99)] private float _sneakSpeed;

    [SerializeField] private float _rotationSpeed;

    private float _velocityMagnitudeFraction;

    [SerializeField] private LocomotionInput _locomotionInput;
    private CharacterController _characterController;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    public IReadOnlyReactiveProperty<LocomotionState> State => _state;
    public float VelocityMagnitudeFraction => _velocityMagnitudeFraction;

    public float RunSpeed => _runSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float SneakSpeed => _sneakSpeed;
 
    public float MoveSpeed
    {
        get
        {
            switch (_state.Value)
            {
                case LocomotionState.Running:
                    return HorizontalMagnitude * _runSpeed;
                case LocomotionState.Sprinting:
                    return HorizontalMagnitude * _sprintSpeed;
                case LocomotionState.Sneaking:
                    return HorizontalMagnitude * _sneakSpeed;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }
  
    private float HorizontalMagnitude
    {
        get 
        { 
           return Mathf
                .Clamp01(Mathf
                .Abs(Mathf
                .Sqrt(Mathf
                .Pow(_locomotionInput.InputDirection.x, 2)) 
                + Mathf.Pow(_locomotionInput.InputDirection.z, 2)));        
        }
    }


    protected virtual void Awake() => _characterController = GetComponent<CharacterController>();

    private void OnEnable()
    {
        _locomotionInput
            .LocomotionInputAction
            .Subscribe(_ => OnLocomotionInputAction(_.Item1, _.Item2))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();

    protected virtual void Update()
    {
        _characterController.Move(Physics.gravity * Time.deltaTime);
        Move();
        SetVelocityMagnitudeFraction();
    }

    protected void SwitchState(LocomotionState state)
    {
        _state.Value = state;
    }

    private void Move()
    {
        if (_locomotionInput.InputDirection != Vector3.zero)
        {
            if (_characterController.isGrounded == true)
            {
                Debug.Log(MoveSpeed);
                Vector3 velocity = HorizontalMagnitude * MoveSpeed * _locomotionInput.InputDirection.normalized;
                _characterController.Move(velocity * Time.deltaTime);

                Quaternion rotation = Quaternion.LookRotation(_locomotionInput.InputDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnLocomotionInputAction(LocomotionState state, bool status)
    {
        if(status == false)
        {
            _state.Value = LocomotionState.Running;
        }
        else
        {
            _state.Value = state;
        }
    }

    private void SetVelocityMagnitudeFraction()
    {
        if (float.IsNaN(_characterController.velocity.magnitude / MoveSpeed))
        {
            _velocityMagnitudeFraction = 0f;
        }
        else
        {
            _velocityMagnitudeFraction = Mathf.Clamp01(_characterController.velocity.magnitude / MoveSpeed);
        }
    }
}
