using System.Diagnostics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Initial Variable Values


    // References


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function to play the game
    public void Play()
    {
        SceneManager.LoadScene(sceneName: "SampleScene");
    }
    
    // Function to close the game
    public void Quit()
    {
        Application.Quit();
    }
}
