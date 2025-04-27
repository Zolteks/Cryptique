using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PZL_Simon : Puzzle
{
    //------
    // The whole logic is working but we got a few issues with visuals, we aim to fix that whene we got the definitive sprites

    [SerializeField] List<Transform> buttons;
    [SerializeField] int roundAmount = 5;

    [SerializeField] float fShowDuration = 1;
    [SerializeField] float fClickDuration = .2f;
    [SerializeField] float fShowPauseDuration = 2f;

    [SerializeField] Material activatedMat;

    [SerializeField] new Camera camera;
    [SerializeField] Canvas canvas;

    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject righttWall;

    List<int> m_currentLayout;
    int m_playerStreak;
    bool m_busy = false;
    GameObject m_UIPlay;

    public void ResetPuzzle()
    {
        m_currentLayout = new List<int>(roundAmount);
        m_playerStreak = 0;

        PlayNextRound();
    }

    private void Start()
    {
        m_UIPlay = GameObject.Find("UIPlay");
    }

    public void TriggerButton(int id)
    {
        if (m_busy) return;

        StartCoroutine(CoroutineTriggerButton(id));
    }

    IEnumerator CoroutineTriggerButton(int id)
    {
        if (id == m_currentLayout[m_playerStreak])
        {
            StartCoroutine(CoroutineEnlightButton(buttons[id], fClickDuration));
            yield return new WaitForSeconds(fClickDuration);

            ++m_playerStreak;
            if (m_playerStreak >= m_currentLayout.Count)
            {
                if (m_currentLayout.Count < roundAmount)
                    PlayNextRound();
                else
                {
                    ChangeCamState();
                    m_UIPlay.SetActive(true);
                    Complete();
                }
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    public override void Quit()
    {
        PC_PlayerController.Instance.EnableInput();
    }

    void PlayNextRound()
    {
        m_currentLayout.Add(Random.Range(0, buttons.Count));
        m_playerStreak = 0;

        StartCoroutine(CoroutinePlayingRound());
    }

    IEnumerator CoroutinePlayingRound()
    {
        m_busy = true;

        for (int i = 0; i < m_currentLayout.Count; i++)
        {
            //print(m_currentLayout[i] + " : " + buttons[m_currentLayout[i]].name);
            StartCoroutine(CoroutineEnlightButton(buttons[m_currentLayout[i]], fShowDuration));
            yield return new WaitForSeconds(fShowDuration + fShowPauseDuration);
        }

        m_busy = false;
    }

    IEnumerator CoroutineEnlightButton(Transform button, float duration)
    {
        //TODO: this is temp function to show puzzle, meant to be changed when we got sprites
        //TODO: fix visual issues

        var initCol = button.GetComponent<StalactiteBehaviour>().m_initCol;
        float timeSinceBegin = 0;

        while(timeSinceBegin < duration / 2)
        {
            timeSinceBegin += Time.deltaTime;
            float intensity = timeSinceBegin / (duration / 2);
            button.GetComponent<MeshRenderer>().material.SetFloat("_EmissionFactor", intensity);
            yield return null;
        }

        timeSinceBegin = 0;
        while (timeSinceBegin < duration / 2)
        {
            timeSinceBegin += Time.deltaTime;
            float intensity = 1 - timeSinceBegin / (duration / 2);
            button.GetComponent<MeshRenderer>().material.SetFloat("_EmissionFactor", intensity);
            yield return null;
        }

        button.GetComponent<MeshRenderer>().material.SetFloat("_EmissionFactor", 0f);
    }

    public void ChangeCamState()
    {
        Transform cam = camera.transform;
        if (!camera.gameObject.activeSelf)
        {
            camera.gameObject.SetActive(true);
            canvas.gameObject.SetActive(true);

            var allowedRotations = new Dictionary<CameraDirdection, bool>() {
                {CameraDirdection.bot, false },
                {CameraDirdection.right, false },
                {CameraDirdection.top, false },
                {CameraDirdection.left, false },
            };
            GameManager.Instance.GetCamera().GetComponent<CameraRotator>().SetAllowedRotation(allowedRotations);

            leftWall.SetActive(false);
            righttWall.SetActive(false);

            SGL_InteractManager.Instance.ChangeCamera(camera);
        }
        else
        {
            camera.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);

            GameManager.Instance.GetCamera().GetComponent<CameraRotator>().ResetAllowedDirections();
            GameManager.Instance.GetCamera().GetComponent<CameraRotator>().UpdateWalls();
            SGL_InteractManager.Instance.ChangeCamera(Camera.main);
        }
    }
}
