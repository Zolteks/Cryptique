using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Snowman : Puzzle
{
    Transform m_snowBall;

    [SerializeField] float scaleFactor;
    [SerializeField] float initialScale = .1f;

    [SerializeField] Transform head;
    [SerializeField] Transform body;

    [SerializeField] Transform target;

    bool m_bodyOver;

    Vector3 m_lastPos;

    private void Start()
    {
        m_bodyOver = false;
        m_snowBall = body;

        m_lastPos = body.position;

        body.localScale = new Vector3 (initialScale, initialScale, initialScale);
        head.localScale = new Vector3 (initialScale, initialScale, initialScale);
    }

    private void FixedUpdate()
    {
        float dist = Vector3.Magnitude(m_snowBall.position - m_lastPos) * Time.deltaTime;
        m_lastPos = m_snowBall.position;
        if (Physics.Raycast(m_snowBall.position, new Vector3(0, -1, 0), out RaycastHit hit, 100, 512))
        {
            Regress(dist);
        }
        else
            Progress(dist);
    }

    private void Progress(float dist)
    {
        if (m_snowBall.localScale.x < 1)
        {
            float increment = scaleFactor * dist * Mathf.Max((1 - m_snowBall.localScale.x), .1f) ;
            increment = Mathf.Min(1 - m_snowBall.localScale.x, increment);
            m_snowBall.localScale += new Vector3(increment, increment, increment);
            //m_snowBall.GetComponent<Rigidbody>().AddTorque(new Vector3 (increment, 0, 0), ForceMode.Impulse);
            m_snowBall.transform.Rotate(new Vector3(increment * 1000, 0, 0), Space.Self);
        }
        else if(Vector3.Magnitude(target.position - m_snowBall.position) <= 1)
        {
            ValidateStep();
        }
        //print(Vector3.Magnitude(target.position - m_snowBall.position));
    }

    private void Regress(float dist)
    {
        if (m_snowBall.localScale.x > initialScale)
        {
            float increment = scaleFactor * dist * Mathf.Max((initialScale - m_snowBall.localScale.x), .1f);
            increment = Mathf.Max(initialScale - m_snowBall.localScale.x, increment);
            increment *= 10;
            m_snowBall.localScale -= new Vector3(increment, increment, increment);
            m_snowBall.transform.Rotate(new Vector3(increment * 1000, 0, 0), Space.Self);
        }
    }

    private void ValidateStep()
    {
        if (false == m_bodyOver)
        {
            m_snowBall = head;
            m_bodyOver = true;
            m_lastPos = head.position;
        }
        else
            Complete();
    }
}
