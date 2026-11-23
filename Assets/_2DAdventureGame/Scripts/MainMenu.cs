using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public string levelScene = "MyLevel";

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        uiDocument.rootVisualElement.Q<Button>("PlayButton").clicked += StartGame;
    }

    void Update()
    {
        if (Keyboard.current != null
            && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene(levelScene);
    }
}
