using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_GutterLabyrinth : Puzzle
{
    [SerializeField]PipeManager pipeManager;
    [SerializeField]Camera cam;

    private void Start()
    {
        SGL_InteractManager.Instance.ChangeCamera(cam);
        PC_PlayerController.Instance.DisableInput();
        SGL_InteractManager.Instance.EnableInteraction();
    }

    public void Solve()
    {
        if (pipeManager == null)
        {
            Debug.LogWarning("PipeManager.Instance est null !");
            return;
        }

        if (pipeManager.isSolved)
        {
            Debug.Log("Puzzle gutter solved !");
            SGL_InteractManager.Instance.ChangeCamera(Camera.main);
            StartCoroutine(CoroutineSolve());
        }
    }

    IEnumerator CoroutineSolve()
    {
        yield return new WaitForSeconds(1.5f);

        PC_PlayerController.Instance.EnableInput();
        Complete();
    }
}
