using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    public Vector3 FrameVelocity { get; private set; }
    private Vector3 _previousPosition;

    public void Update()
    {
        if(Time.deltaTime != 0)
        {
            Vector3 currentFrameVelocity = (transform.position - _previousPosition) / Time.deltaTime;
            FrameVelocity = Vector3.Lerp(FrameVelocity, currentFrameVelocity, 0.1f);
            _previousPosition = transform.position;
        }
    }
}
