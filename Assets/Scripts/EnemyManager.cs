using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : MonoBehaviour
{
    EnemyMover m_enemyMover;
    EnemySensor m_enemySensor;
    Board m_board;

    private void Awake()
    {
        m_enemyMover = FindObjectOfType<EnemyMover>();
        m_enemySensor = FindObjectOfType<EnemySensor>();
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
    }
}
