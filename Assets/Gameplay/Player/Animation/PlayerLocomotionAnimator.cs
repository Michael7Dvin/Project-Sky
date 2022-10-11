using UnityEngine;

public class PlayerLocomotionAnimator : LocomotionAnimator
{
    protected override void OnLocomotionMoveSpeedTypeChanged(LocomotionMoveSpeedType type)
    {
        switch (type)
        {
            case LocomotionMoveSpeedType.Normal:
                Animator.SetBool("LocomotionIsSprinting", false);
                Animator.SetBool("LocomotionIsSlow", false);
                break;
            case LocomotionMoveSpeedType.Sprint:
                Animator.SetBool("LocomotionIsSprinting", true);
                Animator.SetBool("LocomotionIsSlow", false);
                break;
            case LocomotionMoveSpeedType.Slow:
                Animator.SetBool("LocomotionIsSlow", true);
                Animator.SetBool("LocomotionIsSprinting", false);
                break;
        }
    }

    protected override void OnLocomotionTypeChanged(LocomotionType type)
    {
        switch (type)
        {
            case LocomotionType.Ground:
                Animator.SetBool("LocomotionIsJumping", false);
                Animator.SetBool("LocomotionIsFalling", false);
                break;
            case LocomotionType.Fall:
                Animator.SetBool("LocomotionIsJumping", false);
                Animator.SetBool("LocomotionIsFalling", true);
                break;
            case LocomotionType.Jump:
                Animator.SetBool("LocomotionIsJumping", true);
                Animator.SetBool("LocomotionIsFalling", false);
                break;
        }
    }
}
