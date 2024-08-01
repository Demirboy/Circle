using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPointerColorChange : MonoBehaviour
{
    private Renderer sphereRenderer;
    private Color originalColor;
    public Color highlightColor = Color.red; // Color when the sphere is highlighted
    public bool colorChanged = false;


    private VRPointerInteraction vrPointerInteraction;
    public XRRayInteractor rayInteractor;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        vrPointerInteraction = GetComponent<VRPointerInteraction>();

        if (sphereRenderer != null)
        {
            originalColor = sphereRenderer.material.color;
        }

        if (rayInteractor == null)
        {
            Debug.LogError("Ray Interactor is not assigned!");
        }
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = highlightColor;
            colorChanged = true;
 
        }
        if (vrPointerInteraction != null)
        {
            vrPointerInteraction.OnHoverEnter(args);
        }
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = originalColor;
            colorChanged = false;

            if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
            {
                Vector3 errorDirection = hitInfo.point - transform.position;
                Debug.Log("Error Direction: " + errorDirection);
            }
            else
            {
                Debug.LogWarning("Ray Interactor did not hit anything.");
            }
        }
        if (vrPointerInteraction != null)
        {
            vrPointerInteraction.OnHoverExit(args);
        }
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (vrPointerInteraction != null)
        {
            vrPointerInteraction.OnSelectEnter(args);
        }
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        if (vrPointerInteraction != null)
        {
            vrPointerInteraction.OnSelectExit(args);
        }
    }
}
