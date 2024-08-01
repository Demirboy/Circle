using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class XRStartPosition : MonoBehaviour
{
    public Transform startTransform; // A transform that represents the desired starting position and rotation


    private void Start()
    {
        if (startTransform != null)
        {
            ResetXROrigin();
        }
    }

    private void ResetXROrigin()
    {
        // Get the XR Origin component
        XROrigin xrOrigin = GetComponent<XROrigin>();
        if (xrOrigin != null)
        {
            // Move the XR Origin to the start position
            Vector3 positionOffset = xrOrigin.CameraFloorOffsetObject.transform.position - xrOrigin.transform.position;
            xrOrigin.transform.position = startTransform.position - positionOffset;

            // Rotate the XR Origin to the start rotation
            xrOrigin.transform.rotation = startTransform.rotation;
        }
    }
}
