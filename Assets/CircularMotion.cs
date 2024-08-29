using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class CircularMotion : MonoBehaviour
{
    public Transform center; // Reference to the center point
    public Transform cylinder; // Reference to the cylinder
    public float angularSpeed = 2.0f; // Speed of the rotation
    private string player;

    private VRPointerInteraction vrPointerInteraction;
    public XRRayInteractor rayInteractor;

    private int resetNumber = 0;
    public int countdownValue = 2;
    private int errorCount = 0; // Initial countdown value
    public TMP_Text countdownText; // UI Text element to display the countdown
    public TMP_Text accuracyText; // UI Text element to display the accuracy
    public TMP_Text pointTimeDisplay;
    public TMP_Text totalTimeDisplay;
    public TMP_Text spherePosition;
    public TMP_Text pointerPosition;
    public TMP_Text distance;
    public TMP_Text averageDistance;
    public TMP_Text playerName;
    public TMP_Text RecoveryTimeText;
    public TMP_Text errorsOccured;// UI Text to display the average recovery time

    private float lastPointingEndTime = 0f; // Time when the player stopped pointing
    private float accuracy;
    private float radius;
    private float angle = 0f; // Current angle
    private float pointingTime = 0f; // Time pointing at the sphere in current rotation
    private float totalTime = 0f; // Total time of current rotation
    private bool isPointing = false; // Flag to check if pointing is active
    private bool isCounting = false;
    private bool errorCounted = false;
    private Vector3 initialPosition;

    private List<float> accuracies = new List<float>();
    private List<float> posAccuracies = new List<float>();
    private List<string> rotationData = new List<string>();
    private List<float> recoveryTimes = new List<float>(); // List to store recovery times


    void Start()
    {

        if (countdownText != null)
        {
            countdownText.text = "Rotation Nr.: " + countdownValue.ToString();
        }

        initialPosition = transform.position;

        // Calculate the initial position based on the cylinder's radius
        radius = cylinder.localScale.x * 0.5f;
        angle = -Mathf.PI / 2;
        transform.position = new Vector3(0, -radius, 0) + center.position;

        vrPointerInteraction = GetComponent<VRPointerInteraction>();

        player = PlayerData.playerName;
        playerName.text = "P: " + player;

    }

    void Update()
    {

        if (vrPointerInteraction.isRotating && !isCounting)
        {
            StartCoroutine(CalculateDistance());
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
            transform.position = initialPosition;
            CalculateAndDisplayAccuracy();
            CalculateAndDisplayPositionAccuracy();
            UpdateData();
            countdownValue--;
            UpdateCountdownText();
            angularSpeed += 0.15f;
            ResetTracking();
        }

        if (countdownValue <= 0)
        {
            StopGame();

            foreach (string data in rotationData)
            {
                Debug.Log(data);
            }
        }
    }

    private IEnumerator CalculateDistance()
    {
        // Continuously run the loop
        while (true)
        {
            // Display the sphere position
            spherePosition.text = "Sp. Position: " + transform.position.ToString();
            
            // Perform the raycast and calculate the distance
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
            {
                // Correct the Z value and calculate the distance
                Vector3 zCorrectedPoint = hitInfo.point;
                zCorrectedPoint.z = 0;
                pointerPosition.text = "Point. Position: " + zCorrectedPoint.ToString();
                distance.text = Vector3.Distance(transform.position, zCorrectedPoint).ToString();
                posAccuracies.Add(Vector3.Distance(transform.position, zCorrectedPoint));
                string positional = $"Player: {PlayerData.currentScene}, Rotation: {countdownValue}, Pointer:{zCorrectedPoint}, Sphere:{transform.position}, Time: {totalTime}";
                PlayerData.positionals.Add(positional);
            }

            //evey 20th of a sec
            yield return new WaitForSeconds(0.05f);
        }
    }

    void UpdateData()
    {
  
        string dataLine = $"Player: {PlayerData.playerName}, Scene: {PlayerData.currentScene}, Rotation: {countdownValue}, Total time: {totalTime:F2}, Pointing Time: {pointingTime:F2}, Accuracy: {accuracy:0.00}%, {averageDistance.text}, Error Count: {errorCount}";
        rotationData.Add(dataLine);
        PlayerData.trackingData.Add(dataLine);
        foreach (float recoveryTime in recoveryTimes)
        {
            string recoveryData = $"Player: {PlayerData.playerName}, Scene: {PlayerData.currentScene}, RecoveryTime: {recoveryTime}";
            PlayerData.circleRecoveryData.Add(recoveryData);
        }
        recoveryTimes.Clear();

    }

    public void resetHappened()
    {
        resetNumber++;
        string dataLine = $"{resetNumber} Reset(s) happened";
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
            averageDistance.text = "Avg. Distance: " + averagePositionAccuracy.ToString();
        }
        posAccuracies.Clear();
    }


    void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = "Rotation Nr.: " + countdownValue.ToString();
        }
    }

    void CalculateAndDisplayAccuracy()
    {

        float roundedTotalTime = Mathf.Round(totalTime * 10f) / 10f;
        float roundedPointingTime = Mathf.Round(pointingTime * 10f) / 10f;

        accuracy = (roundedTotalTime > 0) ? (roundedPointingTime / roundedTotalTime) * 100f : 0f;
        accuracies.Add(accuracy);

        if (accuracyText != null)
        {
            accuracyText.text = $"Pointing Accuracy: {accuracy:0.00}%";
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
        errorCount = 0;
    }

    public void StartPointing()
    {
        isPointing = true;

        if (!errorCounted)
        {
            errorCounted = true;

            if (lastPointingEndTime > 0)
            {
                float recoveryTime = Time.time - lastPointingEndTime;
                recoveryTimes.Add(recoveryTime);
                RecoveryTimeText.text = recoveryTime.ToString(); 
            }
        }
    }

    public void StopPointing()
    {
        isPointing = false;

        if (errorCounted)
        {
            errorCount++;
            errorsOccured.text = errorCount.ToString();
            errorCounted = false;
            lastPointingEndTime = Time.time; 
        }
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

        

    }
}


