using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Button : MonoBehaviour
{
    // Ben Soars
    public Animator anim; // animator
    public GameObject[] starstoneInfo;

    public void loadScene (string sceneName) // used for UI button to load levels
    {
        StartCoroutine(beginGame(sceneName)); // start begiun game coroutine
    }

    public IEnumerator beginGame(string sceneName)
    {
        GameObject.Find("Canvas").active = false; // set canvas to false
        GameObject.Find("Camera").GetComponent<Animator>().SetBool("Active", true); // get animator
        yield return new WaitForSeconds(4); // wait 8 seconds for animation to play
        SceneManager.LoadScene(sceneName); // load scene   
    }

    //Kurtis Watson
    public void showStarstone(int id)
    {
        for (int i = 0; i < GameObject.Find("Buttons").GetComponent<UI_Button>().starstoneInfo.Length; i++)
        {
            GameObject.Find("Buttons").GetComponent<UI_Button>().starstoneInfo[i].SetActive(false);
        }

        GameObject.Find("Buttons").GetComponent<UI_Button>().starstoneInfo[id].SetActive(true);
    }

    //Ben Soars
    public void setAnimation(string triggerName)
    {
        anim.SetTrigger(triggerName); // play the passed string in the animator
    }

    public void exitGame() // used for UI button to exit
    {
        Application.Quit(); // quit game
    }

    
}
