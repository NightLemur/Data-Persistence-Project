using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreContainer : MonoBehaviour
{
    public RectTransform Underline;
    public Text ScoresText;
    public Text NamesText;
    public Text DifficultyText;

    // Public Animator Functions
    public void UpdateHighScoreTable()
    {
        string[] names;
        int[] scores;

        // Adjust the Title of the screen accordingly
        switch (HighScoreManager.difficulty)
        {
            case MenuManager.DifficultyLevels.easy:
            default:

                DifficultyText.text = "High Scores: (Easy):";
                Underline.sizeDelta = new Vector2(175f, 2.4f);
                names = HighScoreManager.saveData.EasyPlayerNames;
                scores = HighScoreManager.saveData.EasyPlayerScores;

                break;

            case MenuManager.DifficultyLevels.medium:

                DifficultyText.text = "High Scores: (Medium):";
                Underline.sizeDelta = new Vector2(196f, 2.4f);
                names = HighScoreManager.saveData.MediumPlayerNames;
                scores = HighScoreManager.saveData.MediumPlayerScores;

                break;

            case MenuManager.DifficultyLevels.hard:

                DifficultyText.text = "High Scores: (Hard):";
                Underline.sizeDelta = new Vector2(171f, 2.4f);
                names = HighScoreManager.saveData.HardPlayerNames;
                scores = HighScoreManager.saveData.HardPlayerScores;

                break;

            case MenuManager.DifficultyLevels.superHard:

                DifficultyText.text = "High Scores: (Super Hard):";
                Underline.sizeDelta = new Vector2(235f, 2.4f);
                names = HighScoreManager.saveData.SuperHardPlayerNames;
                scores = HighScoreManager.saveData.SuperHardPlayerScores;

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
}
