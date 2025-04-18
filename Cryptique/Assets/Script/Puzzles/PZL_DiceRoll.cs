using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_DiceRoll : Puzzle
{
    //[SerializeField] int amountOfDices = 3;
    //[SerializeField] List<Transform> diceSpawnPoints;

    [SerializeField] List<GameObject> m_playerDice ;
    [SerializeField] List<GameObject> m_enemyDice ;

    bool m_busy = false;

    int m_lastScore;
    int m_diceOver;
    int m_playerScore;

    public void StartRound(bool playerHasRiggedDice)
    {
        if (m_busy) return;

        playerHasRiggedDice = OBJ_DropOnDiceTable.playerHasDice; // TEMP line for playable v1

        m_busy = true;
        Roll(playerHasRiggedDice ? DieBehaviour.RigState.win : DieBehaviour.RigState.lose, m_playerDice);
        StartCoroutine(CoroutineWaitForEnemy(playerHasRiggedDice));
    }

    public void Roll(DieBehaviour.RigState rigState, List<GameObject> dice)
    {
        m_lastScore = 0;
        m_diceOver = 0;

        //foreach (var dice in m_dice)
        //{
        //    Destroy(dice);
        //}

        for(int i = 0; i < dice.Count; i++)
        {
            //GameObject die = (GameObject)GameObject.Instantiate(Resources.Load("Die"), diceSpawnPoints[i]);
            DieBehaviour dieComp = dice[i].GetComponentInChildren<DieBehaviour>();
            dieComp.rigState = rigState;
            dieComp.Roll();
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
        while(m_diceOver < m_playerDice.Count)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.8f);

        m_playerScore = m_lastScore;

        Roll(playerHasRiggedDice ? DieBehaviour.RigState.lose : DieBehaviour.RigState.win, m_enemyDice);
        StartCoroutine(CoroutineWaitForEnd());
    }

    IEnumerator CoroutineWaitForEnd()
    {
        while (m_diceOver < m_enemyDice.Count)
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
