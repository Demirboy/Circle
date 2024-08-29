using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    public TMP_Text playerName;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.playerName != null) {
            playerName.text = PlayerData.playerName;
          }
    }
}
