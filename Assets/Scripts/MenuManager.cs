using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    int _activeScene;

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
    }

    // Public UI Methods
    public void OnSubmitButtonClicked()
    {
        _activeScene = 1;
        SceneManager.LoadScene(1);
    }

    public void OnHighScoreButtonClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void OnOptionsButtonClicked()
    {
        SceneManager.LoadScene(3);
    }
}
