using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using TMPro;

public class VRPointerAndHandMover : MonoBehaviour
{
    private Renderer sphereRenderer;
    private Color originalColor;
    public Color highlightColor = Color.red; 
    public bool colorChanged = false;
    public bool isRotating = false;
    public float errorMargin = 0.1f;
    private VRPointerInteraction vrPointerInteraction;

    public XRRayInteractor rayInteractor;
    public Transform handTransform;

    public GameObject arrowPrefab;
    private GameObject arrowInstance;
    private ArrowController arrowController;

    public float maxRotationAngle = 10.0f; 
    public float maxErrorDistance = 2.0f;
    public float scalingFactor = 1.5f;
    public float maxDistance = 5f;
    public bool resetState = false;



    private Quaternion originalHandRotation;


    public TMP_Text errorTimeText;
    private float totalErrorCorrectionTime = 0.0f;
    private int errorCount = 0;


    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        vrPointerInteraction = GetComponent<VRPointerInteraction>();

        if (sphereRenderer != null)
        {
            originalColor = sphereRenderer.material.color;
        }


        originalHandRotation = handTransform.localRotation;

        if (arrowPrefab != null)
        {
            arrowInstance = Instantiate(arrowPrefab);
            arrowController = arrowInstance.GetComponent<ArrowController>();
        }
    }

    void Update()
    {
        isRotating = vrPointerInteraction.isRotating;

        if (isRotating && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
        {
            Vector3 errorDirection = hitInfo.point - transform.position;
            float errorDistance = errorDirection.magnitude;

            if (errorDistance < maxDistance && !resetState)

            {
                AddError(errorDirection, errorDistance);
            } 

            else

            {
                handTransform.localRotation = originalHandRotation;
                resetState = true;
            }
        }
        else
        {
           
            handTransform.localRotation = originalHandRotation;

        }
    }

    private void AddError(Vector3 errorDirection, float errorDistance)
    {
        
        if (errorDistance <= errorMargin)
        {
            handTransform.localRotation = originalHandRotation;
            return;
        }
        


        float normalizedError = Mathf.Clamp01((errorDistance - errorMargin) / (maxErrorDistance - errorMargin));

        
        float rotationAngle = Mathf.Pow(normalizedError, scalingFactor) * maxRotationAngle;

      
        Vector3 rotationAxis = Vector3.Cross(transform.forward, errorDirection).normalized;

        
        handTransform.localRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis) * originalHandRotation;
    }

    private void ShowArrow(Vector3 targetPosition, Vector3 startPosition)
    {
        if (arrowInstance != null && arrowController != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
        {
            hitInfo.point = startPosition;
            arrowController.endPoint.position = targetPosition;
            arrowInstance.SetActive(true);
        }
    }

    private void HideArrow()
    {
        if (arrowInstance != null)
        {
            arrowInstance.SetActive(false);
        }
    }


    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        resetState = false;

        if (sphereRenderer != null)
        {
            //sphereRenderer.material.color = highlightColor;
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
            //sphereRenderer.material.color = originalColor;
            colorChanged = false;
        }
        //errorStartTime = Time.time;
        errorCount++;


        if (vrPointerInteraction != null)
        {
            vrPointerInteraction.OnHoverExit(args);
        }
    }

    /*
    private void StartRotatingHand(Vector3 errorDirection)
    {
        if (!isMoving && !isReturning)
        {
            StartCoroutine(RotateHandContinuously(errorDirection));
        }
    }

    private IEnumerator RotateHandContinuously(Vector3 errorDirection)
    {
        isMoving = true;
        Quaternion originalRotation = handTransform.localRotation;
        Quaternion initialRotation = handTransform.localRotation;


        while (!colorChanged)
        {
            // Calculate the target rotation for the hand based on error direction
            Quaternion targetRotation = Quaternion.LookRotation(errorDirection.normalized);
            // Limit the rotation to the maximum angle
            Quaternion limitedRotation = Quaternion.RotateTowards(initialRotation, targetRotation, maxRotationAngle);

            handTransform.localRotation = Quaternion.Slerp(handTransform.localRotation, limitedRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        isReturning = true;
        StartCoroutine(ReturnHandToOriginalRotation());
    }
    */

    /*
    private IEnumerator ReturnHandToOriginalRotation()
    {

        
        Quaternion currentHandRotation = handTransform.localRotation;

        while (Quaternion.Angle(handTransform.localRotation, originalHandRotation) > 0.1f)
        {
            handTransform.localRotation = Quaternion.RotateTowards(handTransform.localRotation, originalHandRotation, returnSpeed * Time.deltaTime);
            yield return null;
        }
       

        handTransform.localRotation = originalHandRotation;

        yield return new WaitForSeconds(0.5f);

        isReturning = false;
        isMoving = false;

    }
    */


    /*

    private void StartMovingHand(Vector3 errorDirection)
    {
        if (!isMoving && !isReturning)
        {
            StartCoroutine(MoveHandContinuously(errorDirection));
        }
    }

    private IEnumerator MoveHandContinuously(Vector3 errorDirection)
    {
        isMoving = true;

        while (!colorChanged)
        {
            handTransform.localPosition += errorDirection.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        isReturning = true;
        StartCoroutine(ReturnHandToOriginalPosition());
    }

    private IEnumerator ReturnHandToOriginalPosition()
    {
        isReturning = true;
        Vector3 currentHandPosition = handTransform.localPosition;

        while (Vector3.Distance(handTransform.localPosition, originalHandPosition) > 0.01f)
        {
            handTransform.localPosition = Vector3.MoveTowards(handTransform.localPosition, originalHandPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        handTransform.localPosition = originalHandPosition;
        isReturning = false;
    }

    */
    public void DisplayErrorCorrectionTime()
    {
        float averageErrorCorrectionTime = 0.0f;
        if (errorCount > 0)
        {
            averageErrorCorrectionTime = totalErrorCorrectionTime / errorCount;
        }
        if (errorTimeText != null)
        {
            errorTimeText.text = "Average Error Correction Time: " + averageErrorCorrectionTime.ToString("F2") + "s";
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
