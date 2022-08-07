using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    public static MainManager Instance;
    public string PlayerName;
    public int HighPoints;
    public Text highScoreText;
    public GameObject highScoreName;

    static bool isNameSaved;
    public GameObject highScoreNameSave;

    
    

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadScore();
        //HighPoints=0;
        highScoreText.text=$"Best Score : {PlayerName} : {HighPoints.ToString()}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if(m_Points>HighPoints)
        {
        isNameSaved=false;
        HighPoints=m_Points;
        StartCoroutine("GetName");
        
        }
    }

    [System.Serializable] class SaveData
    {
    public string PlayerName;
    public int HighPoints;
    }

    public void SaveScore()
    {
    SaveData data = new SaveData();
    data.PlayerName = PlayerName;
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

        PlayerName = data.PlayerName;
        HighPoints=data.HighPoints;
    }
    }

    IEnumerator GetName()
    {
        highScoreNameSave.gameObject.SetActive(true);
        yield return new WaitUntil(()=>isNameSaved);
        highScoreNameSave.gameObject.SetActive(false);
        SaveScore();
        
    }

    public void SaveName()
    {
        TMP_InputField tMP_Input = highScoreName.GetComponent<TMP_InputField>();
        PlayerName=tMP_Input.text;
        isNameSaved=true;
        //Debug.Log(PlayerName);
    }
}
