using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_LaunchAnimBumper : MonoBehaviour
{
    private Animator aAnimator;

    public string sNameBumperBool;
    public Bumper bLeftBumper;
    public Bumper bRightBumper;

    private void Start()
    {
        aAnimator = GetComponent<Animator>();
    }

    public void ActivateBumperAnimation()
    {
        aAnimator.SetBool(sNameBumperBool, true);

        

        StartCoroutine(StopAnimation(0.4f));
    }

    private IEnumerator StopAnimation(float delay)
    {
        // Attend avant de commencer la remise en place
        yield return new WaitForSeconds(delay);

        aAnimator.SetBool(sNameBumperBool, false);
    }
}
