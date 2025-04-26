using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AnimatorRandomBehaviour : StateMachineBehaviour
{
    [Header("Number of iterations before random")]
    public int iterationBeforeRandom = 3;

    [Header("Triggers parameters list")]
    public List<string> randomTriggerNames = new List<string>();

    private int m_completedLoops;
    private bool m_randomPlayed;
    private List<int> m_randomTriggerHashes = new List<int>();

    // pour mémoriser sur quelle boucle on était
    private int m_lastLoopCount = 0;

    private void Awake()
    {
        m_randomTriggerHashes.Clear();
        foreach (string triggerName in randomTriggerNames)
        {
            m_randomTriggerHashes.Add(Animator.StringToHash(triggerName));
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_completedLoops = 0;
        m_lastLoopCount = 0;
        m_randomPlayed = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);

        if (currentLoop > m_lastLoopCount)
        {
            m_lastLoopCount = currentLoop;
            m_completedLoops++;

            Debug.Log("Loop terminée : " + m_completedLoops + "/" + iterationBeforeRandom);
        }

        if (m_completedLoops >= iterationBeforeRandom && !m_randomPlayed && m_randomTriggerHashes.Count > 0)
        {
            m_randomPlayed = true;

            int randomIndex = Random.Range(0, m_randomTriggerHashes.Count);
            Debug.Log("Random trigger envoyé : " + randomTriggerNames[randomIndex]);

            animator.SetTrigger(m_randomTriggerHashes[randomIndex]);
        }
    }
}