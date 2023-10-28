using Assets.Scripts.IAJ.Unity.DecisionMaking.RL;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class QTableSerializer
{
    /*
    public static void SaveQTableToFile(Q_Table qTable, string filePath)
    {
        try
        {
            string json = JsonSerializer.Serialize(qTable);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving Q_Table: " + ex.Message);
        }
    }

    public static Q_Table LoadQTableFromFile(string filePath)
    {
        Q_Table qTable = null;

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                qTable = JsonSerializer.Deserialize<Q_Table>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading Q_Table: " + ex.Message);
        }

        return qTable;
    }
    */
}