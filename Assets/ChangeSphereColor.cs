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
        originalColor = sphereRenderer.material.color;
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        sphereRenderer.material.color = highlightColor;
        colorChanged = true;
        vrPointerInteraction.OnHoverEnter(args);
    }

    public void OnHoverExit(HoverExitEventArgs args)
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

         vrPointerInteraction.OnHoverExit(args);
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
