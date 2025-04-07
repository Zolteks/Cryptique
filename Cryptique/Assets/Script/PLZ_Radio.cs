using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLZ_Radio : Puzzle
{
    [SerializeField] MeshRenderer frequenceDisplay;
    [SerializeField] GameObject buttonW;
    [SerializeField] GameObject buttonH;
    Material shader;

    float curFreqW;
    float curFreqH;

    float targetFreqW;
    float targetFreqH;

    void Start()
    {
        shader = frequenceDisplay.materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if ()
        {

        }
    }
}
