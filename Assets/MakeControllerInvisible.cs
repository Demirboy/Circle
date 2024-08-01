using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeControllerInvisible : MonoBehaviour
{
    public Renderer controller;
    public Renderer fakeController;
    public Renderer fakeHandle;
    public Renderer handle;
    public HandleReturnXR handleReturnXR;
    public bool isHeld = false;

    void Start()
    {
        handleReturnXR = GetComponent<HandleReturnXR>();
    }

    void Update()
    {
        isHeld = handleReturnXR.isHeld;

        if (isHeld)
        {
            SetRendererVisibility(controller, false);
            SetRendererVisibility(fakeController, true);
            SetRendererVisibility(handle, false);
            SetRendererVisibility(fakeHandle, true);
        }
        else
        {
            SetRendererVisibility(controller, true);
            SetRendererVisibility(fakeController, false);
            SetRendererVisibility(handle, true);
            SetRendererVisibility(fakeHandle, false);
        }
    }

    private void SetRendererVisibility(Renderer renderer, bool visibility)
    {
        if (renderer != null)
        {
            renderer.enabled = visibility;

            // Iterate over all child renderers and set their visibility
            foreach (Renderer childRenderer in renderer.GetComponentsInChildren<Renderer>())
            {
                childRenderer.enabled = visibility;
            }
        }
    }
}
