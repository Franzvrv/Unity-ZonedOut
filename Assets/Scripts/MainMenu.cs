using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;
    public void PlayGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quitted Game");
    }
}
