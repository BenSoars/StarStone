using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Button : MonoBehaviour
{
    // Ben Soars
    public Animator anim; // animator
    

    public void loadScene (string sceneName) // used for UI button to load levels
    {
        StartCoroutine(beginGame());
    }

    public IEnumerator beginGame()
    {
        GameObject.Find("Canvas").active = false;
        GameObject.Find("Camera").GetComponent<Animator>().SetBool("Active", true);
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("Opening_Scene"); // load scene   
    }

    public void setAnimation(string triggerName)
    {
        anim.SetTrigger(triggerName); // play the passed string in the animator
    }

    public void exitGame() // used for UI button to exit
    {
        Application.Quit(); // quit game
    }

    
}
