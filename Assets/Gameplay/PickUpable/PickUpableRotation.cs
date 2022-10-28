using UnityEngine;
using DG.Tweening; 

public class PickUpableRotation : MonoBehaviour
{
    private Vector3 _initialRotation;

    private Tween YRotate
    {
        get
        {
            return transform
                .DORotate(new Vector3(_initialRotation.x, _initialRotation.y - 360f, _initialRotation.z), 5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Flash)
                .SetLoops(-1)
                .SetUpdate(UpdateType.Normal)
                .SetLink(gameObject);
        }
    }

    private void OnEnable()
    {
        _initialRotation = transform.rotation.eulerAngles;
        YRotate.Play();
    }
}
