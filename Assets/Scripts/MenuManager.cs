using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public InputField PlayerNameInput; // The UI Element where the players enters his name
    public Text ErrorText; // Error text that is made visible if the player does not properly enter a name

    public static MenuManager instance;

    private string _playerName; // The Name the Player has entered

    public string PlayerName { get => _playerName; set => _playerName = value; }

    private void Awake()
    {
        // Ensuring that when this Scene is loaded, MenuManager is not duplicated
        if (instance != null)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerNameInput.text = _playerName;
        ErrorText.gameObject.SetActive(false);
    }

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
            SceneManager.LoadScene(1);
        }
    }
}
