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

    [SerializeField] Transform cameraSpot;
    [SerializeField] new Camera camera;
    Vector3 baseCamSpot;
    Quaternion baseCamRot;
    float baseSize;

    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject righttWall;

    List<int> m_currentLayout;
    int m_playerStreak;
    bool m_busy = false;

    public void ResetPuzzle()
    {
        m_currentLayout = new List<int>(roundAmount);
        m_playerStreak = 0;

        PlayNextRound();
    }

    //private void Start()
    //{
    //    ResetPuzzle();
    //}

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
                    Complete();
                }
            }
        }
        else
        {
            ResetPuzzle();
        }
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
        if (baseCamSpot != cam.position)
        {
            baseCamSpot = cam.position;
            baseCamRot = cam.rotation;
            cam.transform.position = cameraSpot.position;
            cam.transform.rotation = cameraSpot.rotation;

            var allowedRotations = new Dictionary<CameraDirdection, bool>() {
                {CameraDirdection.bot, false },
                {CameraDirdection.right, false },
                {CameraDirdection.top, false },
                {CameraDirdection.left, false },
            };
            GameManager.Instance.GetCamera().GetComponent<CameraRotator>().SetAllowedRotation(allowedRotations); 

            leftWall.SetActive(false);
            righttWall.SetActive(false);

            baseSize = camera.orthographicSize;
            camera.orthographicSize = 3;
        }
        else
        {
            cam.transform.position = baseCamSpot;
            cam.transform.rotation = baseCamRot;
            GameManager.Instance.GetCamera().GetComponent<CameraRotator>().ResetAllowedDirections();

            leftWall.SetActive(true);
            righttWall.SetActive(true);

            camera.orthographicSize = baseSize;
        }
    }
}
