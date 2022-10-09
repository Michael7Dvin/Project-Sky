using System;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
public class LocomotionComposition : MonoBehaviour
{
    public Vector3 MoveVelocity;

    private ReactiveProperty<LocomotionType> _currentLocomotionType = new ReactiveProperty<LocomotionType>();
    private ReactiveProperty<LocomotionMoveSpeedType> _currentLocomotionMoveSpeedType = new ReactiveProperty<LocomotionMoveSpeedType>();

    [SerializeField] private LocomotionInput _input;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private readonly CompositeDisposable _fallingObserveDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _landingObserveDisposable = new CompositeDisposable();

    public BaseGroundLocomotion Ground { get; private set; }
    public BaseFallLocomotion Fall { get; private set; }
    public BaseJumpLocomotion Jump { get; private set; }
    public BaseFlyLocomotion Fly { get; private set; }

    [SerializeField] private GroundDetector _groundDetector;

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

    public Vector3 CharacterVelocity => CharacterController.velocity;

    public IReadOnlyReactiveProperty<LocomotionType> CurrentLocomotionType => _currentLocomotionType;
    public IReadOnlyReactiveProperty<LocomotionMoveSpeedType> CurrentLocomotionMoveSpeedType => _currentLocomotionMoveSpeedType;

    public LocomotionInput Input => _input;
    public CharacterController CharacterController { get; private set; }
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

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                CharacterController.Move(MoveVelocity * Time.deltaTime);
                Debug.Log(CharacterVelocity.y);
            })
            .AddTo(_disposable);

        void ObserveFalling()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Mathf.Abs(CharacterVelocity.y - Fall.VerticalMoveSpeed) < 1)
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
                    if (CharacterVelocity.y <= 0 && GroundDetector.IsGrounded.Value == true && _currentLocomotionType.Value != LocomotionType.Ground)
                    {
                        if (_currentLocomotionType.Value == LocomotionType.Jump || _currentLocomotionType.Value == LocomotionType.Fall)
                        {
                            _currentLocomotionType.Value = LocomotionType.Ground;
                            _landingObserveDisposable.Clear();
                            return;
                        }

                        if (_currentLocomotionType.Value == LocomotionType.Fly && Fly.GetType() == typeof(GlideFlyLocomotion))
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

        Ground?.Disable();
        Ground = groundLocomotion;
        Ground.Initialize(this);
    }
    public void ChangeLocomotion(BaseFallLocomotion fallLocomotion)
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
    public void ChangeLocomotion(BaseJumpLocomotion jumpLocomotion)
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
    public void ChangeLocomotion(BaseFlyLocomotion flyLocomotion)
    {
        if (flyLocomotion == null)
        {
            Debug.Log(new ArgumentException());
            return;
        }

        Fly?.Disable();
        Fly = flyLocomotion;
        Fly.Initialize(this);
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
        }

        void OnGroundLocomotion()
        {
            _currentLocomotionType.Value = LocomotionType.Ground;
        }
        void OnFallLocomotion()
        {
            _currentLocomotionType.Value = LocomotionType.Fall;
        }
        void OnJumpLocomotion()
        {
            if(Jump.GetType() == typeof(NoneJumpLocomotion))
            {
                return;
            }
                    
            if(_currentLocomotionType.Value == LocomotionType.Fly)
            {
                return;
            }

            _currentLocomotionType.Value = LocomotionType.Jump;
        }
        void OnFlyLocomotion()
        {
            if (Fly.GetType() == typeof(NoneFlyLocomotion))
            {
                return;
            }

            if (_currentLocomotionType.Value == LocomotionType.Fly)
            {
                _currentLocomotionType.Value = LocomotionType.Fall;
                return;
            }

            _currentLocomotionType.Value = LocomotionType.Fly;
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
