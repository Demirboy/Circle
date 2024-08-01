using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;


public class PointerPositionDisplay : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // Reference to the XRRayInteractor component
    public TMP_Text pointerText; // Reference to the UI Text component


    void Start()
    {
        if (rayInteractor == null)
        {
            Debug.LogError("Hand transform is not assigned!");
            return;
        }
    }
    

    /*
    void Update()
    {
        // Get the hit point from the XRRayInteractor
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
        {
            pointerText.text = hitInfo.point.ToString();
        }
        else
        {
            Debug.LogWarning("Ray Interactor did not hit anything.");
        }
    }


    */
}