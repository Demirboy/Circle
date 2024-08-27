using System.Collections.Generic;

public static class PlayerData
{
    public static string playerName;

    // This can hold all the data you're collecting across scenes
    public static List<string> trackingData = new List<string>();

    public static List<string> circleRecoveryData = new List<string>();
    // Method to clear data when needed (e.g., when restarting the game)
    public static void ClearData()
    {
        trackingData.Clear();
    }
}