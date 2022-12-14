using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Brick BrickPrefab;
    public int LineCount = 3;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreText1;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private static SaveData SavedData;

    [System.Serializable]
    public class SaveData
    {
        public string[] EasyPlayerNames;
        public int[] EasyPlayerScores;
        public string[] MediumPlayerNames;
        public int[] MediumPlayerScores;
        public string[] HardPlayerNames;
        public int[] HardPlayerScores;
        public string[] SuperHardPlayerNames;
        public int[] SuperHardPlayerScores;

        public SaveData()
        {
            EasyPlayerNames = EmptyArray();
            EasyPlayerScores = new int[10];
            MediumPlayerNames = EmptyArray();
            MediumPlayerScores = new int[10];
            HardPlayerNames = EmptyArray();
            HardPlayerScores = new int[10];
            SuperHardPlayerNames = EmptyArray();
            SuperHardPlayerScores = new int[10];
        }

        public static SaveData Load()
        {
            try
            {
                return JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/DataPersistenceSave.json"));
            }
            catch (FileNotFoundException)
            {
                Debug.Log("MainManager.cs line 57: Save file was not found. Starting with new Save Data.");
                return new SaveData();
            }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(Application.persistentDataPath + "/DataPersistenceSave.json", json);
        }

        private string[] EmptyArray()
        {
            string[] result = new string[10];

            for (int i = 0; i < 10; i++)
            {
                result[i] = "";
            }

            return result;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SavedData = SaveData.Load();
        ScoreText.text = $" {OptionsManager.instance.SavedData.PlayerName} \nScore : 0";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] hitNumArray;
        int[] pointCountArray = new [] {1,1,2,2,5,5,7,7};

        // If the difficulty is Super Hard - some blocks will take multiple hits to break
        if (OptionsManager.instance.SavedData.DifficultyLevel == OptionsManager.DifficultyLevels.superHard)
            hitNumArray = new[] { 1,1,2,2,3,3,4,4};
        else
            hitNumArray = new[] { 1,1,1,1,1,1,1,1};

        // Determine the number of lines in gameplay from the difficulty level selected
        switch(OptionsManager.instance.SavedData.DifficultyLevel)
        {
            case OptionsManager.DifficultyLevels.easy:
                LineCount = 4;
                ScoreText1.text = "Best Score (Easy) : " + (SavedData.EasyPlayerNames[0] == "" ? "N/A" : 
                    SavedData.EasyPlayerNames[0] + " : " + SavedData.EasyPlayerScores[0]);
                break;
            case OptionsManager.DifficultyLevels.medium:
                LineCount = 6;
                ScoreText1.text = "Best Score (Medium) : " + (SavedData.MediumPlayerNames[0] == "" ? "N/A" :
                    SavedData.MediumPlayerNames[0] + " : " + SavedData.MediumPlayerScores[0]);
                break;
            case OptionsManager.DifficultyLevels.hard:
                LineCount = 8;
                ScoreText1.text = "Best Score (Hard) : " + (SavedData.HardPlayerNames[0] == "" ? "N/A" :
                    SavedData.HardPlayerNames[0] + " : " + SavedData.HardPlayerScores[0]);
                break;
            case OptionsManager.DifficultyLevels.superHard:
                LineCount = 8;
                ScoreText1.text = "Best Score (Super Hard) : " + (SavedData.SuperHardPlayerNames[0] == "" ? "N/A" :
                    SavedData.SuperHardPlayerNames[0] + " : " + SavedData.SuperHardPlayerScores[0]);
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

        // Set the Audio Source volume, based on Options Save
        if (OptionsManager.instance.SavedData.BkgdMusicValue > 0)
            audioSource.volume = OptionsManager.instance.SavedData.BkgdMusicValue / 10f;
        else
            audioSource.volume = 0f;

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

                audioSource.Play();
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
        ScoreText.text = $" {OptionsManager.instance.SavedData.PlayerName} \nScore : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Check the High Score list for the current difficulty level. Update as needed
        string scoreText;
        string[] playerNames;
        int[] playerScores;

        switch (OptionsManager.instance.SavedData.DifficultyLevel)
        {
            default:
            case OptionsManager.DifficultyLevels.easy:

                scoreText = "Best Score (Easy) : ";
                playerNames = SavedData.EasyPlayerNames;
                playerScores = SavedData.EasyPlayerScores;

                break;

            case OptionsManager.DifficultyLevels.medium:

                scoreText = "Best Score (Medium) : ";
                playerNames = SavedData.MediumPlayerNames;
                playerScores = SavedData.MediumPlayerScores;

                break;

            case OptionsManager.DifficultyLevels.hard:

                scoreText = "Best Score (Hard) : ";
                playerNames = SavedData.HardPlayerNames;
                playerScores = SavedData.HardPlayerScores;

                break;

            case OptionsManager.DifficultyLevels.superHard:

                scoreText = "Best Score (Super Hard) : ";
                playerNames = SavedData.SuperHardPlayerNames;
                playerScores = SavedData.SuperHardPlayerScores;

                break;
        }

        for (int i = 0; i < 10; i++)
        {
            // There is a player entry here, compare score
            if (playerNames[i] != "")
            {
                // The current player scored higher than this player: Insert the player here, and move the rest down
                if (playerScores[i] < m_Points)
                {
                    for (int j = 9; j >= i; j--)
                    {
                        if (j == i)
                        {
                            playerNames[j] = OptionsManager.instance.SavedData.PlayerName;
                            playerScores[j] = m_Points;
                        }    
                        else
                        {
                            playerNames[j] = playerNames[j - 1];
                            playerScores[j] = playerScores[j - 1];
                        }
                    }

                    if (i == 0) // The Top spot changed: Update the score text now
                        ScoreText1.text = scoreText + OptionsManager.instance.SavedData.PlayerName + " : " + m_Points;

                    SavedData.Save();
                    break;
                }
            }
            else
            {
                if (i == 0) // The Top spot changed: Update the score text now
                    ScoreText1.text = scoreText + OptionsManager.instance.SavedData.PlayerName + " : " + m_Points;

                playerNames[i] = OptionsManager.instance.SavedData.PlayerName;
                playerScores[i] = m_Points;
                SavedData.Save();
                break;
            }
        }

        audioSource.Stop();
    }

    // Public UI Methods
    public void OnReturnButtonClicked()
    {
        // Only return to the main menu if a game is not being played
        if (m_GameOver || !m_Started)
        {
            MenuManager.instance.LastActiveScene = 0;
            SceneManager.LoadScene(0);
        }
    }

    public void OnHighScoresButtonClicked()
    {
        // Only show high scores if a game is not being played
        if (m_GameOver || !m_Started)
            SceneManager.LoadScene(2);
    }
}
