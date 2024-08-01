using System.Collections;
using UnityEngine;

public class EndingTimer : MonoBehaviour
{
    public StartingTimer startingTimer;
    
    public bool isReturning;

    /*
    private void Update()
    {
        isReturning = startingTimer.isReturning;
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        startingTimer.StopTimer();
        /*if (startingTimer != null && startingTimer.timerRunning && !isReturning)
        {
            //startingTimer.isReturning = true;
            HandleStopTimer();
        }
        else if (startingTimer != null && startingTimer.timerRunning && isReturning)
        {
            startingTimer.StartTimer();
        }*/
    }

    

    private IEnumerator HandleStopTimer()
    {     
            startingTimer.StopTimer();
            startingTimer.isReturning = true;
            yield return new WaitForSeconds(2.0f);     
    }
}
