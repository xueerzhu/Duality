using UnityEngine;
using System.IO;
using System.Collections.Generic;


public class DataManager : MonoBehaviour
{
    public SaveData saveData;

    private string file = "SaveData.txt";
    private bool hasSaveFile;

    public void SaveGameAsJSON()
    {
        string json = JsonUtility.ToJson(saveData);
        WriteToFile(file, json);

        Debug.Log("Json Save:" + json);
    }

    public void LoadGameFromJSON()
    {
        saveData = new SaveData();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, saveData);
        Debug.Log("Json Loaded:" + json);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            hasSaveFile = true;
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
            Debug.LogWarning("File Not Found");
        return "";
    }

    public static string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}



[System.Serializable]
public class SaveData
{
    public int maxLevel;
    public int gemTotal;
    public bool isPremiumUser;
    public List<bool> rectangleUnlocks = new List<bool>();
    public List<bool> backgroundUnlocks = new List<bool>();
    public List<bool> floorUnlocks = new List<bool>();
}
