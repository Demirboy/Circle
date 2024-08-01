using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class VRHandMover : MonoBehaviour
{
    public Transform handTransform; // Reference to the hand transform
    public float moveDistance = 1.0f; // Distance to move
    public float moveDuration = 2.0f; // Duration of the movement
    public Vector3 moveDirection = Vector3.forward; // Direction to move

    private bool isMoving = false;

    void Start()
    {
        if (handTransform == null)
        {
            Debug.LogError("Hand transform is not assigned!");
            return;
        }
    }


    public void StartMovingHand()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveHand());
        }
    }

    private IEnumerator MoveHand()
    {
        isMoving = true;
        Vector3 originalPosition = handTransform.position;
        Vector3 targetPosition = originalPosition + moveDirection.normalized * moveDistance;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            handTransform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        handTransform.position = targetPosition;
        isMoving = false;
    }
}
