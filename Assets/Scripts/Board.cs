using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static float spacing = 2f;
    public float
        drawGoalTime = 2f,
        drawGoalDelay = 2f;
    public static readonly Vector2[] directions =
    {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
    };
    public List<Node> AllNodes { get => m_allNodes; }
    public Node PlayerNode { get => m_playerNode; }
    public Node GoalNode { get => m_goalNode; }
    public GameObject goalPrefab;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;

    List<Node> m_allNodes = new List<Node>();
    Node
        m_playerNode,
        m_goalNode;

    PlayerMover m_player;

    private void Awake()
    {
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();
        m_goalNode = FindGoalNode();
    }
    public void GetNodeList()
    {
        Node[] nList = GameObject.FindObjectsOfType<Node>();
        m_allNodes = new List<Node>(nList);
    }

    public Node FindNoteAt(Vector3 pos)
    {
        Vector2 boardCoord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
        return m_allNodes.Find(n => n.Coordinate == boardCoord);
    }

    public Node FindPlayerNode()
    {
        if (m_player && !m_player.isMoving)
        {
            return FindNoteAt(m_player.transform.position);
        }
        return null;
    }

    public void UpdatePlayerNode()
    {
        m_playerNode = FindPlayerNode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        if (m_playerNode)
        {
            Gizmos.DrawSphere(m_playerNode.transform.position, 0.2f);
        }
    }

    Node FindGoalNode()
    {
        return m_allNodes.Find(n => n.isLevelGoal);
    }

    public void DrawGoal()
    {
        if (goalPrefab && m_goalNode)
        {
            GameObject goalInstance = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "time", drawGoalTime,
                "delay", drawGoalDelay,
                "easetype", drawGoalEaseType
                ));
        }
    }

    public void InitBoard()
    {
        if (m_playerNode)
        {
            m_playerNode.InitNode();
        }
    }
}
