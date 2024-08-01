using UnityEngine;
using System.Collections.Generic;

public class PathColliderGenerator : MonoBehaviour
{
    public List<Transform> waypoints; // List of waypoints defining the path
    public float interval = 0.5f; // Distance between sphere colliders
    public float sphereRadius = 0.1f; // Radius of the sphere colliders
    public GameObject sphereColliderPrefab; // Prefab for the sphere colliders

    void Start()
    {
        if (waypoints == null || waypoints.Count < 2)
        {
            Debug.LogError("Insufficient waypoints!");
            return;
        }

        GenerateCollidersAlongPath();
    }

    void GenerateCollidersAlongPath()
    {
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 startPoint = waypoints[i].position;
            Vector3 endPoint = waypoints[i + 1].position;
            float distance = Vector3.Distance(startPoint, endPoint);
            int numSpheres = Mathf.CeilToInt(distance / interval);

            for (int j = 0; j <= numSpheres; j++)
            {
                float t = j / (float)numSpheres;
                Vector3 position = Vector3.Lerp(startPoint, endPoint, t);
                GameObject sphere = Instantiate(sphereColliderPrefab, position, Quaternion.identity);
                sphere.transform.localScale = Vector3.one * sphereRadius * 2; // Set the radius
                sphere.tag = "Wire"; // Tag the sphere collider as "Wire"
                sphere.transform.parent = transform; // Parent to keep the hierarchy clean
            }
        }
    }
}
