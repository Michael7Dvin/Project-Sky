using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
public class LocomotionComposition : MonoBehaviour
{
    private ReactiveProperty<LocomotionType> _currentLocomotionType = new ReactiveProperty<LocomotionType>();
    private ReactiveProperty<LocomotionMoveSpeedType> _currentLocomotionMoveSpeedType = new ReactiveProperty<LocomotionMoveSpeedType>();

    [SerializeField] private LocomotionInput _input;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    public GroundLocomotion Ground { get; private set; }
    public FallLocomotion Fall { get; private set; }
    public JumpLocomotion Jump { get; private set; }

    public float HorizontalInputMagnitude
    {
        get
        {
            return Mathf
                 .Clamp01(Mathf
                 .Abs(Mathf
                 .Sqrt(Mathf.Pow(_input.Direction.x, 2)) + Mathf.Pow(_input.Direction.z, 2)));
        }
    }
    public float VerticalInputMagnitude => Mathf.Clamp01(Mathf.Abs(_input.Direction.y));

    public float CurrentVerticalMoveSpeed
    {
        get
        {
            switch (_currentLocomotionType.Value)
            {
                case LocomotionType.Ground:
                    return Ground.VerticalMoveSpeed;
                case LocomotionType.Fall:
                    return Fall.VerticalMoveSpeed;
            }
            return 0f;
        }
    }
    public float CurrentHorizontalMoveSpeed
    {
        get
        {
            switch(_currentLocomotionType.Value)
            {
                case LocomotionType.Ground:
                    return Ground.HorizontalMoveSpeed;
                case LocomotionType.Fall:
                    return Fall.HorizontalMoveSpeed;
            }
            return 0f;
        }
    }

    public Vector3 CurrentVelocity
    {
        get
        {
            switch (_currentLocomotionType.Value)
            {
                case LocomotionType.Ground:
                    return Ground.Velocity;
                case LocomotionType.Fall:
                    return Fall.Velocity;
            }
            return Vector3.zero;
        }
    }

    public ReactiveProperty<LocomotionType> CurrentLocomotionType => _currentLocomotionType;
    public ReactiveProperty<LocomotionMoveSpeedType> CurrentLocomotionMoveSpeedType => _currentLocomotionMoveSpeedType;

    public LocomotionInput Input => _input;
    public CharacterController CharacterController { get; private set; }

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        Input
            .LocomotionType
            .Subscribe(type => OnInputLocomotionType(type))
            .AddTo(_disposable);

        Input
            .LocomotionMoveSpeedTypeAction
            .Subscribe(action => OnInputLocomotionMoveSpeedAction(action.Item1, action.Item2))
            .AddTo(_disposable);

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                if (Mathf.Abs(Fall.Velocity.y - Fall.VerticalMoveSpeed) < 1)
                {
                    if (_currentLocomotionType.Value == LocomotionType.Ground || _currentLocomotionType.Value == LocomotionType.Jump)
                    {
                        _currentLocomotionType.Value = LocomotionType.Fall;
                    }
                }
                else
                {
                    if (_currentLocomotionType.Value != LocomotionType.Ground)
                    {                        
                        _currentLocomotionType.Value = LocomotionType.Ground;
                    }
                }
            })
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
    private void OnDestroy() => _disposable.Dispose();
    
    public void ChangeGroundLocomotion(GroundLocomotion groundLocomotion)
    {
        if (groundLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        Ground?.Disable();
        Ground = groundLocomotion;
        Ground.Initialize(this);
    }
    public void ChangeFallLocomotion(FallLocomotion fallLocomotion)
    {
        if (fallLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        Fall?.Disable();
        Fall = fallLocomotion;
        Fall.Initialize(this);
    }
    public void ChangeJumpLocomotion(JumpLocomotion jumpLocomotion)
    {
        if (jumpLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        Jump?.Disable();
        Jump = jumpLocomotion;
        Jump.Initialize(this);
    }

    private void OnInputLocomotionType(LocomotionType type)
    {
        if (CharacterController.isGrounded == true)
        {
            _currentLocomotionType.Value = type;
        }
        else if (type == (LocomotionType.Jump | LocomotionType.Fly))
        {
            _currentLocomotionType.Value = type;
        }
    }
    private void OnInputLocomotionMoveSpeedAction(LocomotionMoveSpeedType type, bool status)
    {
        if (status == false)
        {
            _currentLocomotionMoveSpeedType.Value = LocomotionMoveSpeedType.Normal;
        }
        else
        {
            _currentLocomotionMoveSpeedType.Value = type;
        }
    }
}
