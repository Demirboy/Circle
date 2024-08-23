using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // Reference to the center point
    public Transform cylinder; // Reference to the cylinder
    public float angularSpeed = 2.0f; // Speed of the rotation

    private VRPointerInteraction vrPointerInteraction;
    public XRRayInteractor rayInteractor;

    public int countdownValue = 2; // Initial countdown value
    public TMP_Text countdownText; // UI Text element to display the countdown
    public TMP_Text accuracyText; // UI Text element to display the accuracy
    public TMP_Text pointTimeDisplay;
    public TMP_Text totalTimeDisplay;
    public TMP_Text spherePosition;
    public TMP_Text pointerPosition;
    public TMP_Text distance;
    public TMP_Text averageDistance;

    private float radius;
    private float angle = 0f; // Current angle
    private float pointingTime = 0f; // Time pointing at the sphere in current rotation
    private float totalTime = 0f; // Total time of current rotation
    private bool isPointing = false; // Flag to check if pointing is active
    private bool isCounting = false;

    private Vector3 zCorrectedPoint;

    private List<float> accuracies = new List<float>();
    private List<float> posAccuracies = new List<float>();
    private List<string> rotationData = new List<string>();


    void Start()
    {

        if (countdownText != null)
        {
            countdownText.text = countdownValue.ToString();
        }

        // Calculate the initial position based on the cylinder's radius
        radius = cylinder.localScale.x * 0.5f;
        angle = -Mathf.PI / 2;
        transform.position = new Vector3(0, -radius, 0) + center.position;

        vrPointerInteraction = GetComponent<VRPointerInteraction>();
        
    }

    void Update()
    {
        if (vrPointerInteraction.isRotating && !isCounting)
        {
            StartCoroutine(CalculateDistanceEveryQuarterSecond());
            isCounting = true;
        }
        

        if (!enabled || cylinder == null || countdownValue <= 0) return;

        // Calculate the new angle
        angle += angularSpeed * Time.deltaTime;

        // Calculate the new position
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, 0) + center.position;

        // Increment the total time
        totalTime += Time.deltaTime;

        if (isPointing)
        {
            pointingTime += Time.deltaTime;
        }

        // Check if a full rotation is completed
        if (angle >= 3 * Mathf.PI / 2)
        {
            angle -= 2 * Mathf.PI;
            CalculateAndDisplayAccuracy();
            CalculateAndDisplayPositionAccuracy();
            UpdateData();
            countdownValue--;
            UpdateCountdownText();
            ResetTracking();
        }

        if (countdownValue <= 0)
        {
            StopGame();
        }
    }

    private IEnumerator CalculateDistanceEveryQuarterSecond()
    {
        // Continuously run the loop
        while (true)
        {
            // Display the sphere position
            spherePosition.text = transform.position.ToString();

            // Perform the raycast and calculate the distance
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
            {
                // Correct the Z value and calculate the distance
                Vector3 zCorrectedPoint = hitInfo.point;
                zCorrectedPoint.z = 0;
                pointerPosition.text = zCorrectedPoint.ToString();
                distance.text = Vector3.Distance(transform.position, zCorrectedPoint).ToString();
                posAccuracies.Add(Vector3.Distance(transform.position, zCorrectedPoint));
            }

            // Wait for 1/4th of a second (0.25 seconds)
            yield return new WaitForSeconds(0.1f);
        }
    }

    void UpdateData()
    {
        string dataLine = $"Rotation:{countdownValue}, TotalTime: {totalTimeDisplay.text}, PointingTime: {pointTimeDisplay.text}, Average Distance: {averageDistance.text}";
        rotationData.Add(dataLine);
    }

    public void PrintRotationData()
    {
        foreach (string data in rotationData)
        {
            Debug.Log(data);
        }
    }

    void CalculateAndDisplayPositionAccuracy()
    {
        float averagePositionAccuracy = 0f;
        if (posAccuracies.Count > 0)
        {
            averagePositionAccuracy = posAccuracies.Average();
            averageDistance.text = averagePositionAccuracy.ToString();
        }
        posAccuracies.Clear();
    }


    void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = countdownValue.ToString();
        }
    }

    void CalculateAndDisplayAccuracy()
    {
        float accuracy = (totalTime > 0) ? (pointingTime / totalTime) * 100f : 0f;
        accuracies.Add(accuracy);

        if (accuracyText != null)
        {
            accuracyText.text = $"Accuracy: {accuracy:0.00}%";
        }

    }


    void OnGUI()
    {
        if (totalTimeDisplay != null)
        {
            totalTimeDisplay.text = $"Total: " + totalTime;
            pointTimeDisplay.text = $"Pointing: " + pointingTime;
        }
    }

    void ResetTracking()
    {
        pointingTime = 0f;
        totalTime = 0f;
    }

    public void StartPointing()
    {
        isPointing = true;
    }

    public void StopPointing()
    {
        isPointing = false;
    }

    void StopGame()
    {
        enabled = false;

        float averageAccuracy = 0f;
        if (accuracies.Count > 0)
        {
            averageAccuracy = accuracies.Average();
        }

        if (accuracyText != null)
        {
            accuracyText.text = $"Average Accuracy: {averageAccuracy:0.00}%";
        }

        // Get reference to VRPointerAndHandMover and display error correction time
        /*
        VRPointerAndHandMover handMover = GetComponent<VRPointerAndHandMover>();
        if (handMover != null)
        {
            handMover.DisplayErrorCorrectionTime();
        }
        */
    }
}


