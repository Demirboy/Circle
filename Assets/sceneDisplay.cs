using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class sceneDisplay : MonoBehaviour
{
    public TMP_Text sceneName;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        {
            sceneName.text = SceneManager.GetActiveScene().name;
        }
    }
}
