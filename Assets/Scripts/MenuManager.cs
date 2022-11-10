using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public enum DifficultyLevels { easy, medium, hard, superHard };

    public Dropdown DifficultyDropDown; // The UI Element where the user selects the difficulty
    public InputField PlayerNameInput; // The UI Element where the players enters his name
    public Text ErrorText; // Error text that is made visible if the player does not properly enter a name

    public static MenuManager instance;

    private DifficultyLevels _difficultyLevel = DifficultyLevels.easy; // The Difficulty level the player has selected
    private string _playerName; // The Name the Player has entered

    int _activeScene;

    public DifficultyLevels DifficultyLevel { get => _difficultyLevel; set => _difficultyLevel = value; }
    public string PlayerName { get => _playerName; set => _playerName = value; }
    public int LastActiveScene { get => _activeScene; set => _activeScene = value; }

    class SaveData
    {

    }


    private void Awake()
    {
        // Ensuring that when this Scene is loaded, MenuManager is not duplicated
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        _activeScene = SceneManager.GetActiveScene().buildIndex;

        PlayerNameInput.text = _playerName;
        ErrorText.gameObject.SetActive(false);
    }

    // Public UI Methods
    public void OnSubmitButtonClicked()
    {
        // First, we check if we have a valid Player Name
        if (PlayerNameInput.text == "")
        {
            ErrorText.gameObject.SetActive(true);
            return;
        }
        else
        {
            _playerName = PlayerNameInput.text;
            _activeScene = 1;
            SceneManager.LoadScene(1);
        }
    }

    public void OnDropDownChanged()
    {
        switch (DifficultyDropDown.value)
        {
            case 0:
                _difficultyLevel = DifficultyLevels.easy;
                break;
            case 1:
                _difficultyLevel = DifficultyLevels.medium;
                break;
            case 2:
                _difficultyLevel = DifficultyLevels.hard;
                break;
            case 3:
                _difficultyLevel = DifficultyLevels.superHard;
                break;
            default:
                _difficultyLevel = DifficultyLevels.easy;
                break;
        }
    }

    public void OnHighScoreButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
