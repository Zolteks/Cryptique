using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAnimator : MonoBehaviour
{
    public float speed = 2f;
    public GameObject objectToMove;

    public void AnimateFlow(List<PipePieceTrigger> path)
    {
        if (objectToMove == null || path == null || path.Count == 0)
        {
            Debug.LogWarning("FlowAnimator: objectToMove or path is nul !");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(MoveThroughPath(path));
    }

    private IEnumerator MoveThroughPath(List<PipePieceTrigger> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 start = path[i].transform.position;
            Vector3 end = path[i + 1].transform.position;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                objectToMove.transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
        }

        Debug.Log("Animation ended !");
    }
}
