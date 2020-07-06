using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Button : MonoBehaviour
{
    // Ben Soars

    public void loadScene (string sceneName) // used for UI button to load levels
    {
        SceneManager.LoadScene(sceneName); // load scene
    }

    public void exitGame() // used for UI button to exit
    {
        Application.Quit(); // quit game
    }
}
