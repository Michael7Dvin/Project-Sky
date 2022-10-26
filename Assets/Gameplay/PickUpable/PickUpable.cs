using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(PickUpableRotation))]
public class PickUpable : MonoBehaviour
{
    public PickUpableRotation PickUpableLevitation { get; private set; }

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        PickUpableLevitation = GetComponent<PickUpableRotation>();
    }
}
