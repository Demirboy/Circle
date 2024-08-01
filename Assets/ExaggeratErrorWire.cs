using UnityEngine;

public class OldWireLoopError : MonoBehaviour
{
    public Transform ringCenter; // Reference to the RingCenter object
    public float maxOffsetDistance = 1.0f; // Maximum offset distance when the handle is far from the wire
    public float maxDistance = 2.0f; // Maximum distance to calculate the offset
    private Transform handleTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        if (ringCenter == null)
        {
            Debug.LogError("RingCenter not assigned.");
            return;
        }

        handleTransform = transform;
        originalPosition = handleTransform.localPosition;
        originalRotation = handleTransform.localRotation;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wire")) // Assuming the wire has the tag "Wire"
        {
            Vector3 closestPoint = other.ClosestPoint(ringCenter.position);
            float distanceToWire = Vector3.Distance(ringCenter.position, closestPoint);
            ApplyOffset(distanceToWire, closestPoint);
        }
    }

    void ApplyOffset(float distanceToWire, Vector3 closestPoint)
    {
        // Normalize the distance based on the maximum distance
        float normalizedDistance = Mathf.Clamp01(distanceToWire / maxDistance);

        // Calculate the offset based on the normalized distance
        float offsetDistance = normalizedDistance * maxOffsetDistance;

        // Calculate the direction of the offset
        Vector3 offsetDirection = (ringCenter.position - closestPoint).normalized;

        // Apply the offset to the handle's local position
        Vector3 offset = offsetDirection * offsetDistance;
        handleTransform.localPosition = originalPosition + offset;

        // Optionally, you can also exaggerate the rotation based on the distance to the wire
        // Uncomment the following lines if you want to apply rotation
        // Quaternion errorRotation = Quaternion.AngleAxis(offsetDistance, offsetDirection);
        // handleTransform.localRotation = originalRotation * errorRotation;
    }
}