using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Radio : Puzzle
{
    [SerializeField] Camera cam;

    [SerializeField] MeshRenderer frequenceDisplay;
    [SerializeField] PZL_RadioBtn buttonW;
    [SerializeField] PZL_RadioBtn buttonH;
    Material shader;

    public float curFreqW;
    public float curFreqH;

    float targetFreqW;
    float targetFreqH;

    [SerializeField] float minW;
    [SerializeField] float maxW;

    [SerializeField] float minH;
    [SerializeField] float maxH;

    [SerializeField] float toleranceW = .1f;
    [SerializeField] float toleranceH = .05f;

    float timeForValidation = 5f;

    void Start()
    {
        shader = frequenceDisplay.materials[0];
        curFreqW = 2;
        curFreqH = .1f;
        targetFreqW = 5f;
        targetFreqH = .15f;
        shader.SetFloat("_TargetFreqWidth", targetFreqW);
        shader.SetFloat("_TargetFreqHeight", targetFreqH);

        SGL_InteractManager.Instance.ChangeCamera(cam);
    }

    void Update()
    {
        curFreqW = minW + (buttonW.angle + 360) / (720 / maxW);
        curFreqH = minH + (buttonH.angle + 360) / (720 / maxH);

        //print("height: "+curFreqH);
        //print("width: "+curFreqW);

        // Set the shader values base on the current frequence.
        shader.SetFloat("_FreqWidth", curFreqW);
        shader.SetFloat("_FreqHeight", curFreqH);

        // Checks if the current frequence is between acceptable bounds for Width and Height
        if ((curFreqH < targetFreqH + toleranceH && curFreqH > targetFreqH - toleranceH) && (curFreqW < targetFreqW + toleranceW && curFreqW > targetFreqW - toleranceW))
        {
            timeForValidation -= Time.deltaTime;
            if (timeForValidation < 0)
            {
                SGL_InteractManager.Instance.ChangeCamera(Camera.main);
                Complete();
            }
        }
    }
}
