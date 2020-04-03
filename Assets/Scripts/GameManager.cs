using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Board m_board;
    PlayerManager m_player;
    bool
        m_hasLevelStarted = false,
        m_isGamePlaying = false,
        m_isGameOver = false,
        m_hasLevelFinished = false;

    public bool HasLevelStarted { get => m_hasLevelStarted; set => m_hasLevelStarted = value; }
    public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }

    public float
        delayToStart = 1f,
        delayForPlayerInput = 2f;

    public UnityEvent
        setupEvent,
        startLevelEvent,
        playLevelEvent,
        endLevelEvent;

    private void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        m_player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
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
}
