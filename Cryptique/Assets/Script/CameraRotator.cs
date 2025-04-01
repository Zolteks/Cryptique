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
        transform.Rotate(0, -90, 0);
        m_currentDir++;
        if ((int)m_currentDir > 3) m_currentDir = CameraDirdection.bot;
        eDirectionUpdate?.Invoke(m_currentDir);
    }
    public void RotateLeft()
    {
        transform.Rotate(0, 90, 0);
        m_currentDir--;
        if ((int)m_currentDir < 0) m_currentDir = CameraDirdection.left;
        eDirectionUpdate?.Invoke(m_currentDir);
    }
}
