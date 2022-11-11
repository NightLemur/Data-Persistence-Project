using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class OptionsManager : MonoBehaviour
{
    public enum DifficultyLevels { easy, medium, hard, superHard };

    public SaveData SavedData;

    public Button BackButton;
    public Button ForwardButton;
    public InputField PlayerNameInput;
    public Slider BckgdMusicSlider;
    public Slider SFXMusicSlider;
    public Text BkgdTextValue;
    public Text DifficultyText;
    public Text SFXTextValue;

    public static OptionsManager instance;

    // Save Data class that manages Players setting across sessions
    [System.Serializable]
    public class SaveData
    {
        public float BkgdMusicValue;
        public float SfxValue;
        public DifficultyLevels DifficultyLevel;
        public string PlayerName;

        public SaveData()
        {
            BkgdMusicValue = 10f;
            SfxValue = 10f;
            DifficultyLevel = DifficultyLevels.easy;
            PlayerName = "AAA";
        }

        public static SaveData Load()
        {
            try
            {
                return JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/Options.json"));
            }
            catch (FileNotFoundException)
            {
                Debug.Log("OptionsManager.cs line 39: Options file was not found. Starting with new Options Data.");
                return new SaveData();
            }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(Application.persistentDataPath + "/Options.json", json);
        }
    }

    private void Awake()
    {
        // Ensuring that this instance is not duplicated upon reloading
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        SavedData = SaveData.Load();

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            // Initialize UI elements with the saved data
            BckgdMusicSlider.value = SavedData.BkgdMusicValue;
            BkgdTextValue.text = SavedData.BkgdMusicValue.ToString();
            SFXMusicSlider.value = SavedData.SfxValue;
            SFXTextValue.text = SavedData.SfxValue.ToString();
            PlayerNameInput.text = SavedData.PlayerName;

            UpdateDifficultyUI();
        }
    }

    // Changes the "Difficulty Text" Text, and manages visibility of the buttons
    void UpdateDifficultyUI()
    {
        switch (SavedData.DifficultyLevel)
        {
            default:
            case DifficultyLevels.easy:
                DifficultyText.text = "EASY";
                BackButton.gameObject.SetActive(false);
                break;
            case DifficultyLevels.medium:
                DifficultyText.text = "MEDIUM";
                BackButton.gameObject.SetActive(true);
                break;
            case DifficultyLevels.hard:
                DifficultyText.text = "HARD";
                ForwardButton.gameObject.SetActive(true);
                break;
            case DifficultyLevels.superHard:
                DifficultyText.text = "SUPER HARD";
                ForwardButton.gameObject.SetActive(false);
                break;
        }
    }

    // Public UI Methods
    public void OnReturnButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
    public void OnBackgroundMusicSliderChanged()
    {
        SavedData.BkgdMusicValue = BckgdMusicSlider.value;
        BkgdTextValue.text = BckgdMusicSlider.value.ToString();

        SavedData.Save();
    }
    public void OnSFXMusicSliderChanged()
    {
        SavedData.SfxValue = SFXMusicSlider.value;
        SFXTextValue.text = SFXMusicSlider.value.ToString();

        SavedData.Save();
    }
    public void OnBackButtonClicked()
    {
        SavedData.DifficultyLevel--;
        SavedData.Save();

        UpdateDifficultyUI();
    }
    public void OnForwardButtonClicked()
    {
        SavedData.DifficultyLevel++;
        SavedData.Save();

        UpdateDifficultyUI();
    }
    public void OnPlayerNameInputEndEdit()
    {
        SavedData.PlayerName = PlayerNameInput.text;
        SavedData.Save();
    }    
}
