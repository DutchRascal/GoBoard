using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : TurnManager
{
    EnemyMover m_enemyMover;
    EnemySensor m_enemySensor;
    Board m_board;

    protected override void Awake()
    {
        base.Awake();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
        m_board = GetComponent<Board>();
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        if (m_gameManager && !m_gameManager.IsGameOver)
        {
            m_enemySensor.UpdateSensor();
            yield return new WaitForSeconds(0f);

            if (m_enemySensor.FoundPlayer)
            {
                m_gameManager.LoseLevel();
            }
            else
            {
                m_enemyMover.MoveOneTurn();
            }
        }
    }
}
