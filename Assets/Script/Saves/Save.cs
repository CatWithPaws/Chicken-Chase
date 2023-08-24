using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;

public class Save
{
    private static string saveFileName = Application.persistentDataPath +  "/save.sv";

    public static SaveBody saveFile { get; private set; } = new SaveBody();

    public static void LoadSave()
    {
        if (File.Exists(saveFileName))
        {
            ReadSaves();
        }
        else
        {
            CreateNewSave();
        }
    }

    private static void ReadSaves()
    {
        StreamReader sr = new StreamReader(saveFileName);

        saveFile = JsonConvert.DeserializeObject<SaveBody>(sr.ReadToEnd());
        
        sr.Close();
    }

    private static void CreateNewSave()
    {
        StreamWriter sw = new StreamWriter(saveFileName);

        saveFile = new SaveBody();

        sw.Write(JsonConvert.SerializeObject(saveFile));
        sw.Close();
    }

    public static void SaveGame()
    {
        StreamWriter sw = new StreamWriter(saveFileName, false);
        sw.Write(JsonConvert.SerializeObject(saveFile));
        sw.Close();
    }
}
