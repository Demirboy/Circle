using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Spawn : MonoBehaviour
{
    
    public XROrigin xrOrigin; // Reference to your XR Origin object
    public Transform target;  // Reference to the target transform

    void Start()
    {
        // Start the coroutine that will run the movement after a delay
        StartCoroutine(MoveCameraWithDelay(0.1f)); // 1.0f indicates a delay of 1 second
    }

    private IEnumerator MoveCameraWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Execute your original code after the delay
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
    }
}