using UnityEngine;
using DG.Tweening; 

public class PickUpableLevitation : MonoBehaviour
{
    private void OnEnable()
    {
        float positionY = transform.position.y;
        Vector3 rotation = transform.rotation.eulerAngles;

        transform
            .DORotate(new Vector3(rotation.x, rotation.y + 360f, rotation.z), 5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Flash)
            .SetLoops(-1)
            .SetUpdate(UpdateType.Normal)
            .SetLink(gameObject);

        transform
            .DOMoveY(positionY + 0.5f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetUpdate(UpdateType.Normal)
            .SetLink(gameObject);
    }
}
