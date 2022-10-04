using UnityEngine;
using System;

public class GlideFallLocomotion : DefaultFallLocomotion
{
    private readonly float _glideFastSpeed;
    private readonly float _glideFastSpeedMultiplier;
    private readonly float _glideFastSpeedDivider;

    public GlideFallLocomotion(float verticalSpeed, float glideNormalSpeed, float glideFastSpeed, float glideFastSpeedMultiplier, float glideFastSpeedDivider, float rotationSpeed) : base(verticalSpeed, glideNormalSpeed, rotationSpeed)
    {
        _glideFastSpeed = glideFastSpeed;
        _glideFastSpeedMultiplier = glideFastSpeedMultiplier;
        _glideFastSpeedDivider = glideFastSpeedDivider;
    }

    public override float VerticalMoveSpeed
    {
        get
        {
            switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return VerticalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return VerticalSpeed * _glideFastSpeedMultiplier;
                case LocomotionMoveSpeedType.Slow:
                    return VerticalSpeed / _glideFastSpeedDivider;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }

    public override float HorizontalMoveSpeed
    {
        get
        {
            switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return LocomotionComposition.HorizontalInputMagnitude * HorizontalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return LocomotionComposition.HorizontalInputMagnitude * _glideFastSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return LocomotionComposition.HorizontalInputMagnitude * 0f;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }
}
