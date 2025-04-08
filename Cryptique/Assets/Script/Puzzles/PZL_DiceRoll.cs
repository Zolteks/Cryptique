using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_DiceRoll : Puzzle
{
    [SerializeField] int amountOfDices = 3;
    [SerializeField] List<Transform> diceSpawnPoints;

    List<GameObject> m_dices = new(8);

    bool m_busy = false;

    int m_lastScore;
    int m_diceOver;
    int m_playerScore;

    public void StartRound(bool playerHasRiggedDice)
    {
        if (m_busy) return;

        m_busy = true;
        Roll(playerHasRiggedDice ? DieBehaviour.RigState.win : DieBehaviour.RigState.lose);
        StartCoroutine(CoroutineWaitForEnemy(playerHasRiggedDice));
    }

    public void Roll(DieBehaviour.RigState rigState)
    {
        m_lastScore = 0;
        m_diceOver = 0;

        foreach (var dice in m_dices)
        {
            Destroy(dice);
        }

        for(int i = 0; i < amountOfDices; i++)
        {
            GameObject die = (GameObject)GameObject.Instantiate(Resources.Load("Die"), diceSpawnPoints[i]);
            m_dices.Add(die);
            DieBehaviour dieComp = die.GetComponentInChildren<DieBehaviour>();
            dieComp.rigState = rigState;
            dieComp.onRollOver = OnDiceOverCallback;
        }
    }


    bool OnDiceOverCallback(int result)
    {
        m_lastScore += result;
        ++m_diceOver;

        return true;
    }

    IEnumerator CoroutineWaitForEnemy(bool playerHasRiggedDice)
    {
        while(m_diceOver < amountOfDices)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.8f);

        m_playerScore = m_lastScore;

        Roll(playerHasRiggedDice ? DieBehaviour.RigState.regular : DieBehaviour.RigState.win);
        StartCoroutine(CoroutineWaitForEnd());
    }

    IEnumerator CoroutineWaitForEnd()
    {
        while (m_diceOver < amountOfDices)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.8f);

        m_busy = false;
        if (m_playerScore > m_lastScore)
        {
            Complete();
        }
    }
}
