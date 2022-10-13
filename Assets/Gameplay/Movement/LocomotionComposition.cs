using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
public class LocomotionComposition : MonoBehaviour
{
    public Vector3 MoveVelocity;

    private readonly ReactiveProperty<LocomotionType> _currentLocomotionType = new ReactiveProperty<LocomotionType>();
    private readonly ReactiveProperty<LocomotionMoveSpeedType> _currentLocomotionMoveSpeedType = new ReactiveProperty<LocomotionMoveSpeedType>();

    private readonly ReactiveProperty<BaseGroundLocomotion> _ground = new ReactiveProperty<BaseGroundLocomotion>();
    private readonly ReactiveProperty<BaseFallLocomotion> _fall = new ReactiveProperty<BaseFallLocomotion>();
    private readonly ReactiveProperty<BaseJumpLocomotion> _jump = new ReactiveProperty<BaseJumpLocomotion>();
    private readonly ReactiveProperty<BaseFlyLocomotion> _fly = new ReactiveProperty<BaseFlyLocomotion>();
    private readonly ReactiveProperty<BaseDodgeLocomotion> _dodge = new ReactiveProperty<BaseDodgeLocomotion>();

    [SerializeField] private LocomotionInput _input;
    [SerializeField] private GroundDetector _groundDetector;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private readonly CompositeDisposable _fallingObserveDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _landingObserveDisposable = new CompositeDisposable();

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

    public float CurrentVerticalMoveSpeed => _fall.Value.VerticalMoveSpeed;
    public float CurrentHorizontalMoveSpeed
    {
        get
        {
            switch(_currentLocomotionType.Value)
            {
                case LocomotionType.Ground:
                    return _ground.Value.HorizontalMoveSpeed;
                case LocomotionType.Fall:
                    return _fall.Value.HorizontalMoveSpeed;
            }
            return 0f;
        }
    }

    public IReadOnlyReactiveProperty<LocomotionType> CurrentLocomotionType => _currentLocomotionType;
    public IReadOnlyReactiveProperty<LocomotionMoveSpeedType> CurrentLocomotionMoveSpeedType => _currentLocomotionMoveSpeedType;
   
    public IReadOnlyReactiveProperty<BaseGroundLocomotion> Ground => _ground;
    public IReadOnlyReactiveProperty<BaseFallLocomotion> Fall => _fall;
    public IReadOnlyReactiveProperty<BaseJumpLocomotion> Jump => _jump;
    public IReadOnlyReactiveProperty<BaseFlyLocomotion> Fly => _fly;
    public IReadOnlyReactiveProperty<BaseDodgeLocomotion> Dodge => _dodge;

    public CharacterController CharacterController { get; private set; }
    public LocomotionInput Input => _input;
    public GroundDetector GroundDetector => _groundDetector;

    private void Awake() => CharacterController = GetComponent<CharacterController>();
    private void OnEnable()
    {
        Input
            .CurrentLocomotionType
            .Subscribe(type => OnInputLocomotionType(type))
            .AddTo(_disposable);

        Input
            .LocomotionMoveSpeedTypeAction
            .Subscribe(action => OnInputLocomotionMoveSpeedAction(action.Item1, action.Item2))
            .AddTo(_disposable);

        GroundDetector
            .IsGrounded
            .Subscribe(state =>
            {
                if(state == true)
                {
                    _fallingObserveDisposable.Clear();
                }
                else 
                {
                    ObserveLanding();
                    ObserveFalling();
                }
            })
            .AddTo(_disposable);

        // Procedural cohesion - CharacterController.Move subscribe should be last, otherwise CharacterController.velocty returns incorrect results
        Observable
            .EveryUpdate()
            .Subscribe(_ => CharacterController.Move(MoveVelocity * Time.deltaTime))
            .AddTo(_disposable);


        void ObserveFalling()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Mathf.Abs(CharacterController.velocity.y - _fall.Value.VerticalMoveSpeed) < 1)
                    {
                        if (_currentLocomotionType.Value == LocomotionType.Ground || _currentLocomotionType.Value == LocomotionType.Jump)
                        {
                            _currentLocomotionType.Value = LocomotionType.Fall;
                        }
                    }
                })
                .AddTo(_fallingObserveDisposable);

        }
        void ObserveLanding()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {                    
                    if (MoveVelocity.y <= 0 && GroundDetector.IsGrounded.Value == true && _currentLocomotionType.Value != LocomotionType.Ground)
                    {
                        if (_currentLocomotionType.Value == LocomotionType.Jump || _currentLocomotionType.Value == LocomotionType.Fall)
                        {
                            _currentLocomotionType.Value = LocomotionType.Ground;
                            _landingObserveDisposable.Clear();
                            return;
                        }
                    }
                })
                .AddTo(_landingObserveDisposable);
        }

    }

    private void OnDisable() => _disposable.Clear();
    private void OnDestroy() => _disposable.Dispose();
    
    public void ChangeLocomotion(BaseGroundLocomotion groundLocomotion)
    {
        if (groundLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        _ground.Value = groundLocomotion;
        _ground.Value.Initialize(this);
    }
    public void ChangeLocomotion(BaseFallLocomotion fallLocomotion)
    {
        if (fallLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        _fall.Value = fallLocomotion;
        _fall.Value.Initialize(this);
    }
    public void ChangeLocomotion(BaseJumpLocomotion jumpLocomotion)
    {
        if (jumpLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        _jump.Value = jumpLocomotion;
        _jump.Value.Initialize(this);
    }
    public void ChangeLocomotion(BaseFlyLocomotion flyLocomotion)
    {
        if (flyLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        _fly.Value = flyLocomotion;
        _fly.Value.Initialize(this);
    }
    public void ChangeLocomotion(BaseDodgeLocomotion dodgeLocomotion)
    {
        if (dodgeLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        _dodge.Value = dodgeLocomotion;
        _dodge.Value.Initialize(this);
    }

    private void OnInputLocomotionType(LocomotionType type)
    {
        switch(type)
        {
            case LocomotionType.Ground:
                OnGroundLocomotion();
                return;
            case LocomotionType.Fall:
                OnFallLocomotion();
                return;
            case LocomotionType.Jump:
                OnJumpLocomotion();
                return;
            case LocomotionType.Fly:
                OnFlyLocomotion();
                return;
            case LocomotionType.Dodge:
                OnDodgeLocomotion();
                return;
        }

        void OnGroundLocomotion()
        {
            _currentLocomotionType.Value = type;
        }
        void OnFallLocomotion()
        {
            _currentLocomotionType.Value = type;
        }
        void OnJumpLocomotion()
        {
            if(Jump.Value.GetType() == typeof(NoneJumpLocomotion))
            {
                return;
            }
                    
            if(_currentLocomotionType.Value == LocomotionType.Fly || _currentLocomotionType.Value == LocomotionType.Dodge)
            {
                return;
            }

            _currentLocomotionType.SetValueAndForceNotify(type);
        }
        void OnFlyLocomotion()
        {
            if (Fly.Value.GetType() == typeof(NoneFlyLocomotion))
            {
                return;
            }

            if (_currentLocomotionType.Value == LocomotionType.Fly)
            {
                _currentLocomotionType.Value = LocomotionType.Fall;
                return;
            }

            _currentLocomotionType.Value = type;
        }
        void OnDodgeLocomotion()
        {
            if (Dodge.Value.GetType() == typeof(NoneDodgeLocomotion))
            {
                return;
            }

            if (GroundDetector.IsGrounded.Value == false)
            {
                return;
            }

            _currentLocomotionType.Value = type;

            CompositeDisposable _onRollCompletedDisposable = new CompositeDisposable();

            _dodge
                .Value
                .RollCompleted
                .Subscribe(_ => OnRollCompleted())
                .AddTo(_onRollCompletedDisposable);

            void OnRollCompleted()
            {
                _currentLocomotionType.Value = LocomotionType.Ground;
                _onRollCompletedDisposable.Dispose();
            }
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
