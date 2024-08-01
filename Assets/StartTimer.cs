using UnityEngine;
using TMPro;

public class StartingTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public HandleReturnXR handleReturnXR;
    public bool isReturning = false;
    public bool timerRunning = false;
    public float timer = 0.0f;

    /*
    private void Start()
    {
        handleReturnXR = GetComponent<HandleReturnXR>();
    }
    */

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isReturning)
        {
            StartTimer();
        } 
        else
        {
            StopTimer();
            isReturning = false;
        }
    }

    public void StartTimer()
    {
        timer = 0.0f;
        timerRunning = true;
        Debug.Log("Timer started.");
    }

    public void StopTimer()
    {
        timerRunning = false;
        Debug.Log("Final Time: " + timer);

        if (timerText != null)
        {
            timerText.text = "Final Time: " + timer.ToString("F2") + " seconds";
        }
        isReturning = true;

    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + timer.ToString("F2") + " seconds";
        }
    }
}