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
        SetFalse();

        switch (type)
        {
            case LocomotionType.Ground:
                Animator.SetBool("LocomotionIsGround", true);
                break;
            case LocomotionType.Fall:
                Animator.SetBool("LocomotionIsFall", true);
                break;
            case LocomotionType.Jump:
                Animator.SetBool("LocomotionIsJump", true);
                break;
            case LocomotionType.Fly:
                Animator.SetBool("LocomotionIsFly", true);
                break;
            case LocomotionType.Dodge:
                Animator.SetBool("LocomotionIsDodge", true);
                break;
        }
    }

    private void SetFalse()
    {
        Animator.SetBool("LocomotionIsGround", false);
        Animator.SetBool("LocomotionIsFall", false);
        Animator.SetBool("LocomotionIsJump", false);
        Animator.SetBool("LocomotionIsFly", false);
        Animator.SetBool("LocomotionIsDodge", false);
    }
}
