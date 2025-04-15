using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_GutterLabyrinth : Puzzle
{
    [SerializeField]PipeManager pipeManager;
    [SerializeField]Camera cam;

    private void Start()
    {
        InteractManager.Instance.ChangeCamera(cam);
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
            InteractManager.Instance.ChangeCamera(Camera.main);
            StartCoroutine(CoroutineSolve());
        }
    }

    IEnumerator CoroutineSolve()
    {
        yield return new WaitForSeconds(1.5f);

        Complete();
    }
}
