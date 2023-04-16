using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool paused = false;
    private bool gameOver = false;


    void Awake() {
        // if (!Instance) {
        //     Instance = this;
        // }
    }

    public void Pause() {
        if (!gameOver) {
            if (!paused) {
                paused = true;
                AudioListener.pause = true;
                Time.timeScale = 0;
            } else {
                paused = false;
                AudioListener.pause = false;
                Time.timeScale = 1;
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
