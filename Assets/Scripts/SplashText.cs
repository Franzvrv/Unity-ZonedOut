using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplashText : MonoBehaviour
{
    private TMP_Text splashText;
    private float timer = 1f;
    private bool showingText = false;

    public void ShowText(string text) {
        splashText = this.GetComponent<TMP_Text>();
        splashText.text = text;
        if (showingText) {
            timer = 1f;
        }
        else {
            timer = 1f;
            StartCoroutine(ShowText());
        }
    }

    public IEnumerator ShowText() {
        showingText = true;
        splashText.color = new Vector4(1,1,1,1);
        while(timer > 0f) {
            if (timer == 1f) {
                splashText.color = new Vector4(1,1,1,1);
                yield return new WaitForSeconds(1f);
            }
            splashText.color = new Vector4(1,1,1,splashText.color.a - 0.1f);
            timer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        splashText.color = new Vector4(1,1,1,0);
        showingText = false;
        yield break;
    }
}

