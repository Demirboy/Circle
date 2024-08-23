using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class TeleportPlayer : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform target;
    private int timesReset = 0;

    public void Teleport()
    {
        
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
        Debug.Log("Position reset " + timesReset);
        timesReset = +1;
    }
}