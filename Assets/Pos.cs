using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;


public class ControllerPositionDisplay : MonoBehaviour
{
    public Transform handTransform; // Reference to the hand transform
    public TMP_Text positionText; // Reference to the UI Text component


    void Start()
    {
        if (handTransform == null)
        {
            Debug.LogError("Hand transform is not assigned!");
            return;
        }
    }

    void Update()
    {
       positionText.text = handTransform.position.ToString();
    }
}