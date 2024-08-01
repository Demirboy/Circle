using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeChangeColor : MonoBehaviour
{
    public Renderer ringRenderer;
    public Color highlightColor = Color.red; // Color to change to on collision
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = ringRenderer.material.color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wire"))
        {
            // Change the color of the ring
            if (ringRenderer != null)
            {
                ringRenderer.material.color = highlightColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wire"))
        {
            if (ringRenderer != null)
            {
                ringRenderer.material.color = originalColor;
            }
        }
    }
}
