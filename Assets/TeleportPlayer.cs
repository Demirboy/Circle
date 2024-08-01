using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform player; 
    public Transform targetLocation; 

    
    public void Teleport()
    {
        if (player != null && targetLocation != null)
        {
            player.position = targetLocation.position;
            player.rotation = targetLocation.rotation;
        }
        else
        {
            Debug.LogWarning("Player or target location is not assigned.");
        }
    }
}