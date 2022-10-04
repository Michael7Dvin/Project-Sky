using UnityEngine;
using System;

public class GlideFallLocomotion : DefaultFallLocomotion
{
    private readonly float _normalGlideHorizontalSpeed;
    private readonly float _fastGlideHorizontalSpeed;

    private readonly float _normalGlideVerticalSpeed;
    private readonly float _fastGlideVerticalSpeed;

    public GlideFallLocomotion(float normalGlideVerticalSpeed, float fastGlideVerticalSpeed, float normalGlideHorizontalSpeed, float fastGlideHorizontalSpeed, float verticalSpeed, float horizontalSpeed, float rotationSpeed) : base(verticalSpeed, horizontalSpeed, rotationSpeed)
    {
        _normalGlideHorizontalSpeed = normalGlideHorizontalSpeed;
        _fastGlideHorizontalSpeed = fastGlideHorizontalSpeed;

        _normalGlideVerticalSpeed = normalGlideVerticalSpeed;
        _fastGlideVerticalSpeed = fastGlideVerticalSpeed;
    }

    public override float VerticalMoveSpeed
    {
        get
        {
            if(LocomotionComposition.CurrentLocomotionType.Value == LocomotionType.Fall)
            {
                switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
                {
                    case LocomotionMoveSpeedType.Normal:
                        return _normalGlideVerticalSpeed;
                    case LocomotionMoveSpeedType.Sprint:
                        return _fastGlideVerticalSpeed;
                    case LocomotionMoveSpeedType.Slow:
                        return _normalGlideVerticalSpeed;
                }

                Debug.LogException(new ArgumentException());
                return 0f;
            }
            else
            {
                return VerticalSpeed;
            }
        }
    }

    public override float HorizontalMoveSpeed
    {
        get
        {
            if (LocomotionComposition.CurrentLocomotionType.Value == LocomotionType.Fall)
            {
                switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
                {
                    case LocomotionMoveSpeedType.Normal:
                        return LocomotionComposition.HorizontalInputMagnitude * _normalGlideHorizontalSpeed;
                    case LocomotionMoveSpeedType.Sprint:
                        return LocomotionComposition.HorizontalInputMagnitude * _fastGlideHorizontalSpeed;
                    case LocomotionMoveSpeedType.Slow:
                        return LocomotionComposition.HorizontalInputMagnitude * _normalGlideHorizontalSpeed;
                }

                Debug.LogException(new ArgumentException());
                return 0f;
            }
            else
            {
                return HorizontalSpeed;
            }
        }
    }
}
