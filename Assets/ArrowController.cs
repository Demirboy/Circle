using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowController : MonoBehaviour
{
    public XRRayInteractor startPoint;
    public Transform endPoint; 
    public GameObject arrowheadPrefab; 

    public Vector3 arrowheadRotationOffset; 
    public float arrowheadDistanceFromSphere = 0.1f; 

    private LineRenderer lineRenderer;
    private GameObject arrowheadInstance;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;

        if (arrowheadPrefab != null)
        {
            arrowheadInstance = Instantiate(arrowheadPrefab);
            arrowheadInstance.SetActive(false); 
        }
    }

    void Update()
    {
        if (startPoint.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo) && endPoint != null)
        {
            lineRenderer.SetPosition(0, hitInfo.point);
            lineRenderer.SetPosition(1, endPoint.position);

            
            Collider sphereCollider = endPoint.GetComponent<Collider>();
            if (sphereCollider != null && hitInfo.collider == sphereCollider)
            {
                HideArrowhead();
            }
            else
            {
                ShowArrowhead(hitInfo.point, endPoint.position);
            }
        }
        else
        {
            HideArrowhead();
        }
    }

    private void ShowArrowhead(Vector3 startPoint, Vector3 endPoint)
    {
        if (arrowheadInstance != null)
        {
            arrowheadInstance.SetActive(true);

            
            Vector3 direction = endPoint - startPoint;

           
            Vector3 arrowheadPosition = endPoint + direction.normalized * arrowheadDistanceFromSphere;
            arrowheadInstance.transform.position = arrowheadPosition;

           
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            arrowheadInstance.transform.rotation = lookRotation * Quaternion.Euler(arrowheadRotationOffset);
        }
    }

    private void HideArrowhead()
    {
        if (arrowheadInstance != null)
        {
            arrowheadInstance.SetActive(false);
        }
    }
}
