using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public Animator highScoreAnimator;
    public Button BackButton;
    public Button ForwardButton;
    public HighScoreContainer HighScoreContainer;

    public static MainManager.SaveData saveData;
    public static MenuManager.DifficultyLevels difficulty;

    // Initialize the High Score board based on current difficulty
    void Start()
    {
        difficulty = MenuManager.instance.DifficultyLevel;
        saveData = MainManager.SaveData.Load();
        HighScoreContainer.UpdateHighScoreTable();
        HandleButtonEnabling();
    }

    // Disables/Enabled the Back and Forward arrow buttons as Player switches between high score lists
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
        HandleButtonEnabling();
        highScoreAnimator.SetTrigger("FadeOutLeft");
    }

    public void OnForwardButtonClicked()
    {
        difficulty++;
        HandleButtonEnabling();
        highScoreAnimator.SetTrigger("FadeOutRight");
    }
}
