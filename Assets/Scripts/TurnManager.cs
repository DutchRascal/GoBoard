using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    protected GameManager m_gameManager;
    protected bool m_isTurnComplete = false;

    public bool IsTurnComplete { get => m_isTurnComplete; set => m_isTurnComplete = value; }

    protected virtual void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public virtual void FinishTurn()
    {
        m_isTurnComplete = true;
        if (m_gameManager)
        {
            m_gameManager.UpdateTurn();
        }
    }
}
