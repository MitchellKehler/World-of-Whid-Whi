using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour
{
    /*
     * TODO
     * 
     * Still need to deal with closing messages early and messages that don't close automatically.
     * Need to add the button to display the message again.
     * Should probably use a better system for waiting.
     * 
     */
    
    bool AutoFadeOut;
    float Timer = 0f;

    public void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                AutoFadeOut = false;
                FadeOutText();
            }
        }
    }

    public void FadeInText(string NewText, bool fadeOut)
    {
        AutoFadeOut = fadeOut;
        this.GetComponentInChildren<Text>().text = NewText;
        StartCoroutine(FadeTextToFullAlpha(.1f));
        Timer = GameManager.TEXT_FADE_TIME;
        // Wait
        // StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
    }

    public void FadeOutText()
    {
        Timer = 0;
        StartCoroutine(FadeTextToZeroAlpha(.1f));

    }

    public IEnumerator FadeTextToFullAlpha(float t)
    {
        Image i = this.gameObject.GetComponent<Image>();
        Text txt = this.GetComponentInChildren<Text>();
        while (i.color.a < 1.0f)
        {
            //Debug.Log("i.color.a" + i.color.a);
            Color c = i.color;
            Color c2 = txt.color;
            c.a = c.a + (Time.deltaTime / t);
            c2.a = c.a;
            i.color = c;
            txt.color = c2;
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t)
    {
        Image i = this.gameObject.GetComponent<Image>();
        Text txt = this.GetComponentInChildren<Text>();
        while (i.color.a > 0.0f)
        {
            //Debug.Log("i.color.a" + i.color.a);
            Color c = i.color;
            Color c2 = txt.color;
            c.a = c.a - (Time.deltaTime / t);
            c2.a = c.a;
            i.color = c;
            txt.color = c2;
            yield return null;
        }
    }
}