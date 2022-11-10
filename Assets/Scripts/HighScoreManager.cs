using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public Button BackButton;
    public Button ForwardButton;
    public RectTransform Underline;
    public Text ScoresText;
    public Text NamesText;
    public Text DifficultyText;

    MainManager.SaveData saveData;
    MenuManager.DifficultyLevels difficulty;

    // Start is called before the first frame update
    void Start()
    {
        difficulty = MenuManager.instance.DifficultyLevel;
        saveData = MainManager.SaveData.Load();
        UpdateHighScoreTable();

        HandleButtonEnabling();
    }

    private void UpdateHighScoreTable()
    {
        string[] names;
        int[] scores;

        // Adjust the Title of the screen accordingly
        switch (difficulty)
        {
            case MenuManager.DifficultyLevels.easy:
            default:

                DifficultyText.text = "High Scores: (Easy):";
                Underline.sizeDelta = new Vector2(175f, 2.4f);
                names = saveData.EasyPlayerNames;
                scores = saveData.EasyPlayerScores;

                break;

            case MenuManager.DifficultyLevels.medium:

                DifficultyText.text = "High Scores: (Medium):";
                Underline.sizeDelta = new Vector2(196f, 2.4f);
                names = saveData.MediumPlayerNames;
                scores = saveData.MediumPlayerScores;

                break;

            case MenuManager.DifficultyLevels.hard:

                DifficultyText.text = "High Scores: (Hard):";
                Underline.sizeDelta = new Vector2(171f, 2.4f);
                names = saveData.HardPlayerNames;
                scores = saveData.HardPlayerScores;

                break;

            case MenuManager.DifficultyLevels.superHard:

                DifficultyText.text = "High Scores: (Super Hard):";
                Underline.sizeDelta = new Vector2(235f, 2.4f);
                names = saveData.SuperHardPlayerNames;
                scores = saveData.SuperHardPlayerScores;

                break;
        }

        string namesText = "";
        string scoresText = "";

        // Populate the names and scores from the Save Data
        for (int i = 0; i < 10; i++)
        {
            namesText += names[i] == "" ? "AAA\n" : names[i] + "\n";
            scoresText += scores[i] + "\n";
        }

        NamesText.text = namesText;
        ScoresText.text = scoresText;
    }

    private void HandleButtonEnabling()
    {
        BackButton.interactable = difficulty != MenuManager.DifficultyLevels.easy;
        ForwardButton.interactable = difficulty != MenuManager.DifficultyLevels.superHard;
    }

    // Public UI Methods
    public void OnReturnButtonClicked()
    {
        SceneManager.LoadScene(MenuManager.instance.LastActiveScene);
    }

    public void OnBackButtonClicked()
    {
        difficulty--;
        UpdateHighScoreTable();

        HandleButtonEnabling();
    }

    public void OnForwardButtonClicked()
    {
        difficulty++;
        UpdateHighScoreTable();

        HandleButtonEnabling();
    }
}
