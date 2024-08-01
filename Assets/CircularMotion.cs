using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // Reference to the center point
    public Transform cylinder; // Reference to the cylinder
    public float angularSpeed = 2.0f; // Speed of the rotation

    public int countdownValue = 2; // Initial countdown value
    public TMP_Text countdownText; // UI Text element to display the countdown
    public TMP_Text accuracyText; // UI Text element to display the accuracy
    public TMP_Text pointTimeDisplay;
    public TMP_Text totalTimeDisplay;


    private float angle = 0f; // Current angle

    private float pointingTime = 0f; // Time pointing at the sphere in current rotation
    private float totalTime = 0f; // Total time of current rotation
    private bool isPointing = false; // Flag to check if pointing is active

    private List<float> accuracies = new List<float>();


    void Start()
    {
        if (center == null)
        {
            center = new GameObject("Center").transform;
            center.position = Vector3.zero;
        }

        if (cylinder == null)
        {
            Debug.LogError("No cylinder");
            return;
        }

        if (countdownText != null)
        {
            countdownText.text = countdownValue.ToString();
        }

        // Calculate the initial position based on the cylinder's radius
        float radius = cylinder.localScale.x * 0.5f;

        angle = -Mathf.PI / 2;
 

        transform.position = new Vector3(0, -radius, 0) + center.position;
    }

    void Update()
    {
        if (!enabled || cylinder == null || countdownValue <= 0) return;

        // Get the radius from the cylinder's scale
        float radius = cylinder.localScale.x * 0.5f;

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
            countdownValue--;
            UpdateCountdownText();
            CalculateAndDisplayAccuracy();
            ResetTracking();
        }

        if (countdownValue <= 0)
        {
            StopGame();
        }
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
        VRPointerAndHandMover handMover = GetComponent<VRPointerAndHandMover>();
        if (handMover != null)
        {
            handMover.DisplayErrorCorrectionTime();
        }
    }
}


