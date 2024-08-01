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
        if (circularMotion != null)
        {
            circularMotion.enabled = false; // Disable rotation at start
        }

        pointerColorChange = GetComponent<VRPointerAndHandMover>();
        if (pointerColorChange == null)
        {
            Debug.LogError("VRPointerColorChange component not found!");
        }
    }

    void Update()
    {
        if (pointerColorChange == null) return;

        // Check if the sphere is being pointed at
        bool isPointingAtObject = pointerColorChange.colorChanged;

        // Get the input device
        InputDevice device = InputDevices.GetDeviceAtXRNode(xrNode);

        // Check if the trigger is pressed
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed && !isRotating && isPointingAtObject)
            {
                StartRotation();
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
            circularMotion.enabled = true; // Enable rotation
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
