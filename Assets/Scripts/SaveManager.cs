using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public string PlayerName;
    public string HighName;
    public int HighPoints;
    
    

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    

    [System.Serializable] class SaveData
    {
    public string HighName;
    public int HighPoints;
    }

    public void SaveScore()
    {
    SaveData data = new SaveData();
    data.HighName = HighName;
    data.HighPoints=HighPoints;

    string json = JsonUtility.ToJson(data);
  
    File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
    string path = Application.persistentDataPath + "/savefile.json";
    if (File.Exists(path))
    {
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        HighName = data.HighName;
        HighPoints=data.HighPoints;
    }
    }

    public void SaveName()
    {
        TMP_InputField tMP_Input = MainMenuManager.Instance.highScoreName.GetComponent<TMP_InputField>();
        PlayerName=tMP_Input.text;
    }

    
}
