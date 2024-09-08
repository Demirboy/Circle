using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fpsDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    private float fps;
    public TMP_Text fpsCounter;
    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }

    // Update is called once per frame
    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = "FPS: " + fps.ToString();
    }
}
