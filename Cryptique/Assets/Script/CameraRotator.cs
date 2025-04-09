using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraDirdection
{
    bot,
    right,
    top,
    left,
}
public delegate void DirUpdateDelegate(CameraDirdection newDir);

public class CameraRotator : MonoBehaviour
{
    public event DirUpdateDelegate eDirectionUpdate;

    CameraDirdection m_currentDir;
    bool m_busy = false;

    private void Start()
    {
        m_currentDir = CameraDirdection.bot;
        eDirectionUpdate?.Invoke(m_currentDir);
    }

    void Update()
    {
        // Debug controls for rotation
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            RotateRight();
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            RotateLeft();
#endif
    }

    public void RotateRight()
    {
        if (m_busy) return;

        StartCoroutine(CoroutineRotate(transform.rotation, transform.rotation * Quaternion.Euler(0, -90, 0), .5f, 1));
    }
    public void RotateLeft()
    {
        if (m_busy) return;

        StartCoroutine(CoroutineRotate(transform.rotation, transform.rotation * Quaternion.Euler(0, 90, 0), .5f, -1));
    }
    IEnumerator CoroutineRotate(Quaternion start, Quaternion end, float duration, int incrementValue)
    {
        m_busy = true;
        float t = 0;

        while(t <= duration/2)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(start, end, t/ duration);
            yield return null;
        }

        m_currentDir += incrementValue;
        if ((int)m_currentDir < 0) m_currentDir = CameraDirdection.left;
        else if ((int)m_currentDir > 3) m_currentDir = CameraDirdection.bot;
        eDirectionUpdate?.Invoke(m_currentDir);

        while (t <= duration)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(start, end, t / duration);
            yield return null;
        }

        m_busy = false;
    }
}
