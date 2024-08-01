using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingArrowChangeColor : MonoBehaviour
{
    public Renderer ringRenderer;
    public Color highlightColor = Color.red;
    private Color originalColor;
    public Transform ringMidpoint;
    public LineRenderer lineRenderer;
    public float detectionRadius = 0.5f;
    public Transform handle;
    public HandleReturnXR handleReturnXR;
    public bool isHeld = false;
    private Vector3 originalHandleposition;

    public GameObject arrowheadPrefab;
    public Vector3 arrowheadRotationOffset;
    public float arrowheadDistanceFromSphere = 0.1f;
    private GameObject arrowheadInstance;

    // Start is called before the first frame update
    void Start()
    {
        handleReturnXR = GetComponent<HandleReturnXR>();

        if (ringRenderer != null)
        {
            originalColor = ringRenderer.material.color;
        }

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not assigned");
        }
        else
        {
            lineRenderer.positionCount = 2; 
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); 
            lineRenderer.startColor = Color.yellow; 
            lineRenderer.endColor = Color.yellow; 
        }

        originalHandleposition = handle.position;

        if (arrowheadPrefab != null)
        {
            arrowheadInstance = Instantiate(arrowheadPrefab);
            arrowheadInstance.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        isHeld = handleReturnXR.isHeld;

        if (isHeld)
        {
            Vector3 closestPointOnWire = GetClosestPointOnWire(ringMidpoint.position);
            DrawLineToClosestPoint(closestPointOnWire);
            float lineLength = CalculateLineLength(ringMidpoint.position, closestPointOnWire);
        }

        else
        {
            handle.position = originalHandleposition;
            arrowheadInstance.SetActive(false);
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
            if (arrowheadInstance != null)
            {
                arrowheadInstance.SetActive(false);
            }
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
            ShowArrowhead(ringMidpoint.position, closestPoint);
        }

        
    }

    private void ShowArrowhead(Vector3 startPoint, Vector3 endPoint)
    {
        if (arrowheadInstance != null)
        {
            arrowheadInstance.SetActive(true);


            Vector3 direction = endPoint - startPoint;


            Vector3 arrowheadPosition = endPoint + direction.normalized * arrowheadDistanceFromSphere;
            arrowheadInstance.transform.position = arrowheadPosition;


            Quaternion lookRotation = Quaternion.LookRotation(direction);
            arrowheadInstance.transform.rotation = lookRotation * Quaternion.Euler(arrowheadRotationOffset);
        }
    }

        private float CalculateLineLength(Vector3 startPoint, Vector3 endPoint)
    {
        return Vector3.Distance(startPoint, endPoint);
    }
}
