using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class DataSaver : MonoBehaviour
{
    public void SaveDataToTextFile()
    {
        string name = PlayerData.playerName;
        string path = Path.Combine(Application.dataPath, "../" + name + "TrackingData" + ".txt");

        using (StreamWriter writer = new StreamWriter(path, true)) // 'true' to append data
        {
            foreach (string data in PlayerData.trackingData)
            {
                writer.WriteLine(data);
            }
        }

        string path = Path.Combine(Application.dataPath, "../" + name + "QuestionaireAnswers" + ".txt");

        using (StreamWriter writer = new StreamWriter(path, true)) // 'true' to append data
        {
            foreach (string data in PlayerData.QuestionaireAnswers)
            {
                writer.WriteLine(data);
            }
        }

        string path = Path.Combine(Application.dataPath, "../" + name + "Positionals" + ".txt");
        using (StreamWriter writer = new StreamWriter(path, true)) // 'true' to append data
        {
            foreach (string data in PlayerData.positionals)
            {
                writer.WriteLine(data);
            }
        }
        
        string path = Path.Combine(Application.dataPath, "../" + name + "CircleRecoveryData" + ".txt");
        using (StreamWriter writer = new StreamWriter(path, true)) // 'true' to append data
        {
            foreach (string data in PlayerData.circleRecoveryData)
            {
                writer.WriteLine(data);
            }
        }
        string path = Path.Combine(Application.dataPath, "../" + name + "RingErrors" + ".txt");
        using (StreamWriter writer = new StreamWriter(path, true)) // 'true' to append data
        {
            foreach (string data in PlayerData.ringErrors)
            {
                writer.WriteLine(data);
            }
        }





        Debug.Log("Data saved to " + Path.GetFullPath(path));
    }
}