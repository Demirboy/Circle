using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class DataSaver : MonoBehaviour
{
    public void SaveDataToTextFile()
    {
        string name = PlayerData.playerName;
        string path = Path.Combine(Application.dataPath, "../" + name + ".txt");

        using (StreamWriter writer = new StreamWriter(path, true)) // 'true' to append data
        {
            foreach (string data in PlayerData.trackingData)
            {
                writer.WriteLine(data);
            }
        }

        Debug.Log("Data saved to " + Path.GetFullPath(path));
    }
}