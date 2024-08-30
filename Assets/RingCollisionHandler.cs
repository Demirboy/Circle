using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class RingCollisionHandler : MonoBehaviour
{
    public Renderer ringRenderer;
    public Renderer fakeRingRenderer;
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
    public TMP_Text distanceText;
    public TMP_Text timeSinceStart;
    public TMP_Text lineLengthC;
    public TMP_Text errorTimeText;
    public TMP_Text errorsOccuredText;
    public bool isinside;
    public bool hasTrackingStarted = false;

    private float errorCooldownTimer = 0f; // Timer to track time since last error was counted
    public float errorCooldownInterval = 0.5f; // Cooldown interval in seconds

    public bool displayArrow = true;
    public GameObject arrowheadPrefab;
    public Vector3 arrowheadRotationOffset;
    public float arrowheadDistanceFromSphere = 0.1f;
    private GameObject arrowheadInstance;
    public float startWidth = 0.01f;
    public float endWidth = 0.02f;

    public Transform start; // Reference to the start object
    public Transform end;   // Reference to the end object
    public float trackingDistanceStart = 0.08f; // Distance from start to begin tracking
    public float trackingDistanceEnd = 0.08f; // Distance from end to stop tracking

    private float trackingTime = 0f;
    private float errorStateTime = 0f;
    private float cumulativeLineLength = 0f;
    private int lineLengthSamples = 0;
    private int errorsOccured = 0;
    private bool isTracking = false;
    private string trackingData = "";
    private Vector3 closestPointOnWire;

    private void Start()
    {
        PlayerData.currentScene = SceneManager.GetActiveScene().name;
        
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
            lineRenderer.positionCount = 2; 
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
        }
        if (arrowheadPrefab != null)
        {
            arrowheadInstance = Instantiate(arrowheadPrefab);
            arrowheadInstance.SetActive(false);
        }

        originalHandleposition = handle.position;
    }

    private void Update()
    {

        isHeld = handleReturnXR.isHeld;

        if (isHeld)
        {
            IsMidpointInsideWire();
            closestPointOnWire = GetClosestPointOnWire(ringMidpoint.position);
            float lineLength = CalculateLineLength(ringMidpoint.position, closestPointOnWire) + 0.01f;
            if (lineLength > 0.01f && displayArrow)
            {
                DrawLineToClosestPoint(closestPointOnWire);
            }
            else
            {
                HideArrow();
            };
            distanceText.text = "Line Length: " + lineLength.ToString();
            Vector3 lineDirection = CalculateLineDirection(ringMidpoint.position, closestPointOnWire);
            distanceToFake = CalculateDistanceToFake(ringMidpoint.position, fakeMidpoint.position);
            ApplyExaggerationEffect(lineLength, lineDirection);

            TrackMovement(lineLength);
            string positionals = $"Player: {PlayerData.playerName}, {PlayerData.currentScene}, Midpoint: {ringMidpoint.position}, Closest Point Wire {closestPointOnWire}, Time: {trackingTime}";
            PlayerData.positionals.Add(positionals);
        } 
            else
        {
            handle.position = originalHandleposition;
            HideArrow();
        }
    }

    private bool IsMidpointInsideWire()
    {
        Collider[] colliders = Physics.OverlapSphere(ringMidpoint.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Wire") || collider.CompareTag("Start"))
            {
                // Check if the fakeMidpoint is within the bounds of the wire's collider
                if (collider.bounds.Contains(ringMidpoint.position))
                {
                    isinside = true;
                    return true;
                }

                // For more precise checking (useful for non-box colliders)
                Vector3 closestPoint = collider.ClosestPoint(ringMidpoint.position);
                if (closestPoint == ringMidpoint.position)
                {
                    isinside = true;
                    return true; // fakeMidpoint is inside the collider
                }
            }
        }

        isinside = false;
        return false; // fakeMidpoint is not inside any wire collider
    }

    private void TrackMovement(float lineLength)
    {
        float distanceToStart = Vector3.Distance(ringMidpoint.position, start.position);
        float distanceToEnd = Vector3.Distance(ringMidpoint.position, end.position);

        if (!isTracking && distanceToStart > trackingDistanceStart && !hasTrackingStarted)
        {
            isTracking = true;
            hasTrackingStarted = true;
        }

        if (isTracking)
        {

            // Track time
            trackingTime += Time.deltaTime;
            timeSinceStart.text = "Time: " + trackingTime.ToString();

            // Update the cooldown timer
            errorCooldownTimer += Time.deltaTime;

            // Track time in error state (highlight color)
            if (ringRenderer.material.color == highlightColor)
            {
                errorStateTime += Time.deltaTime;
                errorTimeText.text = "Error Time: " + errorStateTime.ToString();

                // Count error if cooldown has elapsed
                if (errorCooldownTimer >= errorCooldownInterval)
                {
                    errorsOccured++;
                    errorsOccuredText.text = "Errors Occured: " + errorsOccured.ToString();

                    errorCooldownTimer = 0f; // Reset cooldown timer
                    string errorPositions = $"Player: {PlayerData.playerName}, Scene: {PlayerData.currentScene}, Midpoint: {ringMidpoint.position}, Closest Point Wire {closestPointOnWire}, Time: {trackingTime}, Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    PlayerData.ringErrors.Add(errorPositions);
                }
            }


            // Track cumulative line length
            cumulativeLineLength += lineLength;
            lineLengthC.text = "Length Sum: " + cumulativeLineLength.ToString();
            lineLengthSamples++;

            // Automatically stop tracking when reaching the end
            if (distanceToEnd <= trackingDistanceEnd)
            {
                isTracking = false;
                FinalizeTrackingData();
                enabled = false;
            }
        }
    }

    public void FinalizeTrackingData()
    {
        float averageLineLength = GetAverageLineLength();
        trackingData += $"Player: {PlayerData.playerName}, Scene: {PlayerData.currentScene}, Total Time: {trackingTime}, Total Error Time: {errorStateTime}, Cumulative Distance: {cumulativeLineLength}, Average Distance: {averageLineLength}, Errors Occured: {errorsOccured}, Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        Debug.Log(trackingData);
        PlayerData.trackingData.Add(trackingData);
    }

    private float GetAverageLineLength()
    {
        return lineLengthSamples > 0 ? cumulativeLineLength / lineLengthSamples : 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wire"))
        {
            // Change the color of the ring
            if (ringRenderer != null)
            {
                ringRenderer.material.color = highlightColor;
                fakeRingRenderer.material.color = highlightColor;
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
                fakeRingRenderer.material.color = originalColor;
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
            if (collider.CompareTag("Wire") || collider.CompareTag("Start"))
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
        if (lineRenderer != null && displayArrow)
        {
            if (displayArrow)
            {
                lineRenderer.enabled = true;
            };
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
    
    private void HideArrow()
    {
        arrowheadInstance.SetActive(false);
        lineRenderer.enabled = false;
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

    /*
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
    */

    private void ApplyExaggerationEffect(float lineLength, Vector3 lineDirection)
    {
        
        float dynamicOffset = Mathf.Max(exaggerationOffset * distanceToFake, exaggerationOffset);

        
        float exaggerationFactor = Mathf.Clamp01((lineLength - exaggerationThreshold) / maxOffset) * scalingFactor;

       
        Vector3 exaggeratedPosition = handle.position + (-lineDirection * dynamicOffset * exaggerationFactor);

        
        fakeHandle.position = Vector3.Lerp(fakeHandle.position, exaggeratedPosition, Time.deltaTime * moveSpeed);
    }
}
