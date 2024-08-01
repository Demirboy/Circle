using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleReturnXR : MonoBehaviour
{
    public Transform originalPosition; // The original position of the handle
    public StartingTimer startingTimer;
    public Transform returnPosition;
    public bool isReturning = false;
    public float returnSpeed = 5f; // The speed at which the handle returns to its original position

    private XRGrabInteractable grabInteractable;
    public bool isHeld = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }


    void Update()
    {
        //isReturning = startingTimer.isReturning;

        if (!isHeld && !isReturning)
        {
            // Move the handle back to its original position
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalPosition.rotation, returnSpeed * Time.deltaTime);
        }

    }


    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;

    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
