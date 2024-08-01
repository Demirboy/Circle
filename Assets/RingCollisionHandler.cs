using UnityEngine;

public class RingCollisionHandler : MonoBehaviour
{
    public Renderer ringRenderer;
    public Color highlightColor = Color.red; 
    private Color originalColor;
    public Transform ringMidpoint; 
    public Transform fakeMidpoint;
    public LineRenderer lineRenderer; 
    public float detectionRadius = 0.5f; // Radius for spherecast to detect colliders
    public float exaggerationThreshold = 0.01f; // Length of the line at which exaggeration starts
    public float exaggerationOffset = 0.5f; // Exaggeration offset amount
    public float maxOffset = 0.1f;
    public float distanceToFake;
    public Transform handle; 
    public Transform fakeHandle;
    public HandleReturnXR handleReturnXR;
    public bool isHeld = false;
    private Vector3 originalHandleposition;
    public float scalingFactor = 1;
    public float moveSpeed = 5.0f;

    private void Start()
    {
        handleReturnXR = GetComponent<HandleReturnXR>();

        if (ringRenderer != null)
        {
            originalColor = ringRenderer.material.color;
        }
        else
        {
            Debug.LogError("Ring not assigned");
        }

        if (ringMidpoint == null)
        {
            Debug.LogError("Midpoint not assigned");
        }

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not assigned");
        }
        else
        {
            lineRenderer.positionCount = 2; // Set the number of positions to 2
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Set a basic material
            lineRenderer.startColor = Color.yellow; // Set start color
            lineRenderer.endColor = Color.yellow; // Set end color
        }

        originalHandleposition = handle.position;
    }

    private void Update()
    {

        isHeld = handleReturnXR.isHeld;

        if (isHeld)
        {
            Vector3 closestPointOnWire = GetClosestPointOnWire(ringMidpoint.position);
            DrawLineToClosestPoint(closestPointOnWire);
            float lineLength = CalculateLineLength(ringMidpoint.position, closestPointOnWire);
            Vector3 lineDirection = CalculateLineDirection(ringMidpoint.position, closestPointOnWire);
            distanceToFake = CalculateDistanceToFake(ringMidpoint.position, fakeMidpoint.position);
            ApplyExaggerationEffect(lineLength, lineDirection);
        } 
            else
        {
            handle.position = originalHandleposition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wire"))
        {
            // Change the color of the ring
            if (ringRenderer != null)
            {
                ringRenderer.material.color = highlightColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wire"))
        {
            if (ringRenderer != null)
            {
                ringRenderer.material.color = originalColor;
            }

            // Clear the LineRenderer
            lineRenderer.SetPosition(0, ringMidpoint.position);
            lineRenderer.SetPosition(1, ringMidpoint.position);
        }
    }

    private Vector3 GetClosestPointOnWire(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, detectionRadius);
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Wire"))
            {
                Vector3 colliderClosestPoint = collider.ClosestPoint(point);
                float distance = Vector3.Distance(point, colliderClosestPoint);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = colliderClosestPoint;
                }
            }
        }

        return closestPoint;
    }

    private void DrawLineToClosestPoint(Vector3 closestPoint)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, ringMidpoint.position);
            lineRenderer.SetPosition(1, closestPoint);
        }
    }

    private float CalculateLineLength(Vector3 startPoint, Vector3 endPoint)
    {
        return Vector3.Distance(startPoint, endPoint);
    }

    private float CalculateDistanceToFake(Vector3 startPoint, Vector3 endPoint)
    {
        return Vector3.Distance(startPoint, endPoint);
    }


    private Vector3 CalculateLineDirection(Vector3 startPoint, Vector3 endPoint)
    {
        return (endPoint - startPoint).normalized;
    }

    private void ApplyExaggerationEffect(float lineLength, Vector3 lineDirection)
    {
        float dynamicOffset = Mathf.Max(exaggerationOffset * distanceToFake, exaggerationOffset);

        if (lineLength > exaggerationThreshold && distanceToFake < maxOffset)
        {
            Vector3 exaggeratedPosition = handle.position + (-lineDirection * dynamicOffset);

            fakeHandle.position = Vector3.Lerp(fakeHandle.position, exaggeratedPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            fakeHandle.position = Vector3.Lerp(fakeHandle.position, handle.position, Time.deltaTime * moveSpeed);
        }
    }
}
