using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireEnabled : MonoBehaviour
{
    public bool isEnabled = false;
    public GameObject wire;

    // Start is called before the first frame update
    void Start()
    {
        wire.SetActive(isEnabled);
    }

   
}
