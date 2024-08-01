using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireLoopErrorExaggeration : MonoBehaviour
{
    public Transform ringCenter; // Reference to the RingCenter object
    public float maxOffsetDistance = 0.1f; // Maximum offset distance when the handle is far from the wire
    public float ringRadius = 0.5f; // Radius of the ring
    private Transform handleTransform;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        if (ringCenter == null)
        {
            Debug.LogError("RingCenter not assigned.");
            return;
        }

        handleTransform = transform;
        originalLocalPosition = handleTransform.localPosition;
        originalLocalRotation = handleTransform.localRotation;
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component missing.");
        }
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Assuming the wire has colliders and is tagged as "Wire"
            Collider[] wireColliders = Physics.OverlapSphere(ringCenter.position, ringRadius, LayerMask.GetMask("Wire"));

            if (wireColliders.Length > 0)
            {
                // Find the closest point on the wire colliders
                Vector3 closestPoint = GetClosestPoint(wireColliders);
                float distanceToWire = Vector3.Distance(ringCenter.position, closestPoint);
                ApplyOffset(distanceToWire, closestPoint);
            }
            else
            {
                // Reset the handle's position and rotation if not close to the wire
                handleTransform.localPosition = originalLocalPosition;
                handleTransform.localRotation = originalLocalRotation;
            }
        }
        else
        {
            // Reset the handle's position and rotation when not held
            handleTransform.localPosition = originalLocalPosition;
            handleTransform.localRotation = originalLocalRotation;
        }
    }

    Vector3 GetClosestPoint(Collider[] colliders)
    {
        Vector3 closestPoint = colliders[0].ClosestPoint(ringCenter.position);
        float closestDistance = Vector3.Distance(ringCenter.position, closestPoint);

        foreach (Collider collider in colliders)
        {
            Vector3 point = collider.ClosestPoint(ringCenter.position);
            float distance = Vector3.Distance(ringCenter.position, point);

            if (distance < closestDistance)
            {
                closestPoint = point;
                closestDistance = distance;
            }
        }

        return closestPoint;
    }

    void ApplyOffset(float distanceToWire, Vector3 closestPoint)
    {
        // Normalize the distance based on the ring radius
        float normalizedDistance = Mathf.Clamp01(distanceToWire / ringRadius);

        // Calculate the offset based on the normalized distance
        float offsetDistance = normalizedDistance * maxOffsetDistance;

        // Calculate the direction of the offset (from wire to the ring center)
        Vector3 offsetDirection = (ringCenter.position - closestPoint).normalized;

        // Apply the offset to the handle's local position
        Vector3 offset = offsetDirection * offsetDistance;
        handleTransform.localPosition = originalLocalPosition + offset;
    }
}
