using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRHandMovementTrigger : MonoBehaviour
{
    public VRHandMover handMover;
    public XRNode xrNode = XRNode.RightHand; // Assume right hand, adjust if needed

    void Update()
    {
        // Get the input device
        InputDevice device = InputDevices.GetDeviceAtXRNode(xrNode);

        // Check if the trigger is pressed
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed) && isTriggerPressed)
        {
            handMover.StartMovingHand();
        }
    }
}