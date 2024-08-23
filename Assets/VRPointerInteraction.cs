using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class VRPointerInteraction : MonoBehaviour
{
    private CircularMotion circularMotion;
    private VRPointerAndHandMover pointerColorChange;
    public bool isRotating = false;
    public XRNode xrNode = XRNode.RightHand;

    void Start()
    {
        circularMotion = GetComponent<CircularMotion>();
        circularMotion.enabled = false; // Disable rotation at start

        pointerColorChange = GetComponent<VRPointerAndHandMover>();
        
    }

    void Update()
    {
        // Check if the sphere is being pointed at
        bool isPointingAtObject = pointerColorChange.colorChanged;

        // Check if the trigger is pressed
        if (isPointingAtObject)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(xrNode);
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
            {
                if (isTriggerPressed && !isRotating)
                {
                    StartRotation();
                }
            }
        }
    }

    void StartRotation()
    {
        if (circularMotion != null)
        {
            isRotating = true;
            circularMotion.enabled = true; // Enable rotation
            circularMotion.StartPointing();
        }
    }

    void StopRotation()
    {
        if (circularMotion != null)
        {
            isRotating = false;
            circularMotion.enabled = false; // Disable rotation
            circularMotion.StopPointing();
        }
    }

    
    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (circularMotion != null)
        {
            isRotating = true;
            circularMotion.enabled = true; 
            circularMotion.StartPointing();
        }
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        if (circularMotion != null)
        {
            isRotating = false;
            circularMotion.enabled = false; // Disable rotation
        }
    }
    

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (circularMotion != null && isRotating)
        {
            circularMotion.StartPointing();
        }
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        if (circularMotion != null)
        {
            circularMotion.StopPointing();
        }
    }

}
