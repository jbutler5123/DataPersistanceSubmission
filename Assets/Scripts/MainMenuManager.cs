using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public GameObject highScoreName;
    public Text highScoreText;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        SaveManager.Instance.LoadScore();
        
        highScoreText.text=$"Best Score : {SaveManager.Instance.HighName} : {SaveManager.Instance.HighPoints.ToString()}";
    }
    public void StartGame()
    {
        SaveManager.Instance.SaveName();
        SceneManager.LoadScene(1);
    }
    public void ExitApp()
    { 
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}
