using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class TeleportPlayer : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform target;
    private int timesReset = 0;


    public void Start()
    {
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
    }
    public void Teleport()
    {
        timesReset = +1;
        xrOrigin.MoveCameraToWorldLocation(target.position);
        xrOrigin.MatchOriginUpCameraForward(target.up, target.forward);
        Debug.Log("Position reset " + timesReset);
        string resetString = "Position reset " + timesReset;
        PlayerData.trackingData.Add(resetString);
    }
}