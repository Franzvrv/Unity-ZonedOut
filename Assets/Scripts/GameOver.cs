using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Animator transitionAnim;
    //public FirstPersonController firstPersonControllerScript;
    //public Player playerScript;
    public void RetryGame()
    {
        //firstPersonControllerScript.enabled = true;
        //playerScript.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LoadMainScene());
    }
    IEnumerator LoadMainScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main Scene");
    }
    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Menu Scene");
    }
}
