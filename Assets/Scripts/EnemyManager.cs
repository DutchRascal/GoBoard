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
        m_enemyMover = FindObjectOfType<EnemyMover>();
        m_enemySensor = FindObjectOfType<EnemySensor>();
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        m_enemySensor.UpdateSensor();
        yield return new WaitForSeconds(0.5f);
        m_enemyMover.MoveOneTurn();
    }
}
