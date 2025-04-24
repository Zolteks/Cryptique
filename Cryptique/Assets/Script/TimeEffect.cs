using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeEffect : MonoBehaviour
{
    int loopAmount = 2;
    float slowFactor = .2f;
    float duration = 2;
    float stayDuration = .6f;
    float endDuration = 2;

    public void TriggerEffect()
    {
        StartCoroutine(CoroutineTimeEffect());
    }

    IEnumerator CoroutineTimeEffect()
    {
        GameObject mask = (GameObject)Instantiate(Resources.Load("WhiteMask"));
        MaskOpacityHandler opacityHandler = mask.GetComponent<MaskOpacityHandler>();

        float loopDuration = duration / (float)loopAmount;
        float slowValue = (1- slowFactor) / loopDuration;
        float accelerationValue = (1- slowFactor) / stayDuration;
        float endOpacityIncrement = (1- slowFactor) / endDuration;
        float loopTimer = 0;

        for(int i = 0; i < loopAmount; i++)
        {
            Time.timeScale = 1;
            loopTimer = 0;
            while(loopTimer < loopDuration)
            {
                loopTimer += Time.unscaledDeltaTime;
                opacityHandler.SetOpacity(loopTimer / loopDuration);
                Time.timeScale -= slowValue * Time.unscaledDeltaTime;
                yield return null;
            }

            loopTimer = 0;
            while(loopTimer < stayDuration)
            {
                loopTimer += Time.unscaledDeltaTime;
                opacityHandler.SetOpacity(1 - loopTimer / stayDuration);
                Time.timeScale += accelerationValue * Time.unscaledDeltaTime;
                yield return null;
            }
        }

        Time.timeScale = 1;
        loopTimer = 0;

        loopTimer = 0;
        while (loopTimer < loopDuration)
        {
            loopTimer += Time.unscaledDeltaTime;
            opacityHandler.SetOpacity(loopTimer / loopDuration);
            Time.timeScale -= slowValue * Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(endDuration);

        //Destroy(mask);

        // Play cinematic

        SceneManager.LoadScene("Bootstrap");
    }
}
