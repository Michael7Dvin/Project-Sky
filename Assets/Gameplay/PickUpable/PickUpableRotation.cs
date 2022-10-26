using UnityEngine;
using DG.Tweening; 

public class PickUpableRotation : MonoBehaviour
{
    private Vector3 _rotation;

    private Tween YRotate
    {
        get
        {
            return transform
                .DORotate(new Vector3(_rotation.x, _rotation.y - 360f, _rotation.z), 5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Flash)
                .SetLoops(-1)
                .SetUpdate(UpdateType.Normal)
                .SetLink(gameObject);
        }
    }

    private void OnEnable()
    {
        _rotation = transform.rotation.eulerAngles;
        YRotate.Play();
    }
}
