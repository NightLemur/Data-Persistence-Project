using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 3;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreText1;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private static SaveData SavedData = new SaveData();

    [System.Serializable]
    class SaveData
    {
        public string PlayerName;
        public int PlayerScore;

        public SaveData()
        {
            PlayerName = "";
            PlayerScore = 0;
        }

        public static SaveData Load()
        {
            try
            {
                return JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/DataPersistenceSave.json"));
            }
            catch (FileNotFoundException)
            {
                Debug.Log("MainManager.cs line 45: Save file was not found. Starting with fresh Save Data.");
                return new SaveData();
            }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(Application.persistentDataPath + "/DataPersistenceSave.json", json);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] hitNumArray;
        int[] pointCountArray = new [] {1,1,2,2,5,5,7,7};

        // If the difficulty is Super Hard - some blocks will take multiple hits to break
        if (MenuManager.instance.DifficultyLevel == MenuManager.DifficultyLevels.superHard)
            hitNumArray = new[] { 1,1,2,2,3,3,4,4};
        else
            hitNumArray = new[] { 1,1,1,1,1,1,1,1};

        // Determine the number of lines in gameplay from the difficulty level selected
        switch(MenuManager.instance.DifficultyLevel)
        {
            case MenuManager.DifficultyLevels.easy:
                LineCount = 4;
                break;
            case MenuManager.DifficultyLevels.medium:
                LineCount = 6;
                break;
            case MenuManager.DifficultyLevels.hard:
            case MenuManager.DifficultyLevels.superHard:
                LineCount = 8;
                break;
            default:
                LineCount = 4;
                break;
        }

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.NumberOfHits = hitNumArray[i];
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        SavedData = SaveData.Load();

        ScoreText1.text = "Best Score : " + (SavedData.PlayerName == "" ? "N/A" : SavedData.PlayerName + " : " + SavedData.PlayerScore);
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        // We have a new high score to save
        if (SavedData.PlayerScore < m_Points)
        {
            SavedData.PlayerName = MenuManager.instance.PlayerName;
            SavedData.PlayerScore = m_Points;

            SavedData.Save();
        }
    }
}
