using System.Collections.Generic;

public static class PlayerData
{
    public static string playerName;

    public static List<string> trackingData = new List<string>();
    public static List<string> circleRecoveryData = new List<string>();
    public static List<string> questionaireAnswers = new List<string>();
    public static List<string> positionals = new List<string>();
    public static List<string> ringErrors = new List<string>();
    public static string currentScene;
    public static void ClearData()
    {
        trackingData.Clear();
    }
}