using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreen : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void StartGame()
    {
        PlayerData.playerName = nameInputField.text;
        Debug.Log(PlayerData.playerName + " saved!");
    }

    public void PrintData()
    {
        string allData = string.Join("\n", PlayerData.trackingData);
        Debug.Log(allData);
    }

    public void clearData()
    {
        PlayerData.ClearData();
        Debug.Log("Data Cleared");
    }

}