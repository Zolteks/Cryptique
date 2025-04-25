using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_GutterLabyrinth : Puzzle
{
    [SerializeField]PipeManager pipeManager;
    [SerializeField]Camera cam;

    private GameObject m_UIPlay;

    private void Awake()
    {
        SGL_InteractManager.Instance.ChangeCamera(cam);
        PC_PlayerController.Instance.DisableInput();
        SGL_InteractManager.Instance.EnableInteraction();

        m_UIPlay = GameObject.Find("UIPlay");
        m_UIPlay.SetActive(false);
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
        m_UIPlay.SetActive(true);
        Complete();
    }
}
