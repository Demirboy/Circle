using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class XRStartPosition : MonoBehaviour
{
    public TeleportPlayer teleportPlayer; 

    private void Start()
    {
        teleportPlayer.Teleport();
    }

}
