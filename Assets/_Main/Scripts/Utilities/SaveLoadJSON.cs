using UnityEngine;
using System.IO;

public class SaveLoadJSON : Singleton<SaveLoadJSON>
{
    public void SaveIntoJsonFile(object obj, string filename){
        string stringObj = JsonUtility.ToJson(obj);
        string _filename = filename + ".json";
        string _path = Path.Combine(Application.persistentDataPath, _filename);

        File.WriteAllText(_path , stringObj);

        Debug.Log(Application.persistentDataPath);       

    }

    public string LoadFromJsonFile(string filename)
    {
        string _filename = filename + ".json";
        string _path = Path.Combine(Application.persistentDataPath, _filename);

        // Does the file exist?
        if (File.Exists(_path))
        {
            // Read the entire file and save its contents.
            string json = File.ReadAllText(_path);
            
            // Return with JSON
            return json;
        }

        return null;
    }
}
