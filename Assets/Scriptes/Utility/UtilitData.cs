using UnityEngine;

public class UtilitData 
{
    public static void SaveData(string json, string nameData = "Player")
    {
        if (!PlayerPrefs.HasKey(nameData))
            PlayerPrefs.SetString(nameData, json);
    }

    public static void SaveData<T>(T data, string nameData = "Player") where T : class
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
            PlayerPrefs.SetString(nameData, json);
    }

    public static void LoadData(string nameData, out string json)
    {
        if (PlayerPrefs.HasKey(nameData))
            json = PlayerPrefs.GetString(nameData);
        else
            json = "";
    }

    private static string LoadData(string nameData)
    {
        if (PlayerPrefs.HasKey(nameData))
            return PlayerPrefs.GetString(nameData);
        else
        {
            PlayerPrefs.SetString(nameData, "");
            return "";
        }
    }

    public static T GetData<T>(string name = "Player") where T : class, new()
    {
        if (LoadData(name) == "") 
            return new T();

        return JsonUtility.FromJson<T>(LoadData(name));
    }
}
