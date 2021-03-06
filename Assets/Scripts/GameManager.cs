﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public enum Turn
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour
{
    Board m_board;
    PlayerManager m_player;
    List<EnemyManager> m_enemies;
    Turn m_currentTurn = Turn.Player;
    bool
        m_hasLevelStarted = false,
        m_isGamePlaying = false,
        m_isGameOver = false,
        m_hasLevelFinished = false;

    public bool HasLevelStarted { get => m_hasLevelStarted; set => m_hasLevelStarted = value; }
    public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }
    public Turn CurrentTurn { get => m_currentTurn; }

    public float
        delayToStart = 1f,
        delayForPlayerInput = 2f;

    public UnityEvent
        setupEvent,
        startLevelEvent,
        playLevelEvent,
        endLevelEvent,
        loseLevelEvent;

    void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        EnemyManager[] enemies = FindObjectsOfType<EnemyManager>() as EnemyManager[];
        m_enemies = enemies.ToList();
    }

    private void Start()
    {
        if (m_player && m_board)
        {
            StartCoroutine(RunGameLoop());
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER Error: no player or board found");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine(StartLevelRoutine());
        yield return StartCoroutine(PlayLevelRoutine());
        yield return StartCoroutine(EndLevelRoutine());
    }

    IEnumerator StartLevelRoutine()
    {
        Debug.Log("SETUP LEVEL");
        if (setupEvent != null)
        {
            setupEvent.Invoke();
        }
        Debug.Log("START LEVEL");
        m_player.playerInput.InputEnabled = false;
        while (!m_hasLevelStarted)
        {
            yield return null;
        }
        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delayToStart);
        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }
        yield return new WaitForSeconds(delayForPlayerInput);
        m_player.playerInput.InputEnabled = true;

        while (!m_isGameOver)
        {
            yield return null;
            m_isGameOver = IsWinner();
        }
        m_player.playerInput.InputEnabled = false;
        Debug.Log("WIN+++++++++======================");
    }

    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");
        m_player.playerInput.InputEnabled = false;
        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }
        while (!m_hasLevelFinished)
        {
            yield return null;
        }
        RestartLevel();
    }

    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    private bool IsWinner()
    {
        if (m_board.PlayerNode)
        {
            return (m_board.PlayerNode == m_board.GoalNode);
        }
        return false;
    }

    public void UpdateTurn()
    {
        if (m_currentTurn == Turn.Player && m_player != null)
        {
            if (m_player.IsTurnComplete && !AreEnemiesAllDead())
            {
                PlayEnemyTurn();
            }

        }
        else if (m_currentTurn == Turn.Enemy)
        {
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }
        }
    }


    void PlayPlayerTurn()
    {
        m_currentTurn = Turn.Player;
        m_player.IsTurnComplete = false;
    }

    void PlayEnemyTurn()
    {
        m_currentTurn = Turn.Enemy;
        foreach (EnemyManager enemy in m_enemies)
        {
            if (enemy && !enemy.IsDead)
            {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }
    }

    bool IsEnemyTurnComplete()
    {
        foreach (EnemyManager enemy in m_enemies)
        {
            if (enemy.IsDead) { continue; }

            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine()
    {
        m_isGameOver = true;
        yield return new WaitForSeconds(1.5f);
        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }
        yield return new WaitForSeconds(2f);
        Debug.Log("LOSE! ===================");
        RestartLevel();
    }

    bool AreEnemiesAllDead()
    {
        foreach (EnemyManager enemy in m_enemies)
        {
            if (!enemy.IsDead) { return false; }
        }
        return true;
    }
}
