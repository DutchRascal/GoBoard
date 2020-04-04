using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Vector2 m_coordinate;
    public Vector2 Coordinate { get => Utility.Vector2Round(m_coordinate); }

    List<Node> m_neighborNodes = new List<Node>();
    public List<Node> NeighborNodes { get => m_neighborNodes; }

    List<Node> m_linkedNodes = new List<Node>();
    public List<Node> LinkedNodes { get => m_linkedNodes; }

    Board m_board;
    bool m_isInitialized = false;

    public GameObject
        geometry,
        linkPrefab;
    public float
        scaleTime = 0.3f,
        delay = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;
    public LayerMask obstacleLayer;
    public bool isLevelGoal = false;

    GameManager m_gamemanager;

    private void Awake()
    {
        m_board = UnityEngine.Object.FindObjectOfType<Board>();
        m_coordinate = new Vector2(transform.position.x, transform.position.z);
        m_gamemanager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        Node[] allNodes;
        allNodes = FindObjectsOfType<Node>();
        int i = 0;
        foreach (Node node in allNodes)
        {
            if (node.isLevelGoal)
            {
                i++;
            }
        }
        if (i > 1 || i == 0)
        {
            Debug.LogWarning("NODE Too much isLevelGoal set or non set");
            Time.timeScale = 0;
        }
        else
        {
            // hide the mesh by scaling to zero
            if (geometry != null)
            {
                geometry.transform.localScale = Vector3.zero;

                // play scale animation at Start

                if (m_board != null)
                {
                    m_neighborNodes = FindNeighbors(m_board.AllNodes);
                }
            }
        }
    }

    // play scale animation
    public void ShowGeometry()
    {
        if (geometry != null)
        {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time", scaleTime,
                "scale", Vector3.one,
                "easetype", easeType,
                "delay", delay
            ));
        }
    }

    public List<Node> FindNeighbors(List<Node> nodes)
    {
        List<Node> nList = new List<Node>();
        foreach (Vector2 dir in Board.directions)
        {
            Node foundNeighbor = FindNeighborAt(nodes, dir);
            if (foundNeighbor != null && !nList.Contains(foundNeighbor))
            {
                nList.Add(foundNeighbor);
            }
        }
        return nList;
    }

    public Node FindNeighborAt(List<Node> nodes, Vector2 dir)
    {
        return nodes.Find(n => n.Coordinate == Coordinate + dir);
    }

    public Node FindNeighborAt(Vector2 dir)
    {
        return FindNeighborAt(NeighborNodes, dir);
    }

    public void InitNode()
    {
        if (!m_isInitialized)
        {
            ShowGeometry();
            InitNeighbors();
            m_isInitialized = true;
        }
    }

    private void InitNeighbors()
    {
        StartCoroutine(InitNeighborsRoutine());
    }

    IEnumerator InitNeighborsRoutine()
    {
        yield return new WaitForSeconds(delay);
        foreach (Node n in m_neighborNodes)
        {
            if (!m_linkedNodes.Contains(n))
            {
                Obstacle obstacle = FindObstacle(n);
                if (!obstacle)
                {
                    LinkNode(n);
                    n.InitNode();
                }
            }
        }
        //StartCoroutine(WaitForIt(2f));
        //print("Finished");
    }

    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("Finished");
    }

    void LinkNode(Node targetNode)
    {
        if (linkPrefab != null)
        {
            GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
            linkInstance.transform.parent = transform;
            Link link = linkInstance.GetComponent<Link>();
            if (link != null)
            {
                link.DrawLink(transform.position, targetNode.transform.position);
            }
            if (!m_linkedNodes.Contains(targetNode))
            {
                m_linkedNodes.Add(targetNode);
            }
            if (!targetNode.LinkedNodes.Contains(this))
            {
                targetNode.LinkedNodes.Add(this);
            }
        }
    }

    Obstacle FindObstacle(Node targetNode)
    {
        Vector3 checkDirection = targetNode.transform.position - transform.position;
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, checkDirection, out raycastHit, Board.spacing + 0.1f, obstacleLayer))
        {
            //Debug.Log("NODE FindObstacle: Hit an obstacle from:" + this.name + " to " + targetNode.name);
            return raycastHit.collider.GetComponent<Obstacle>();
        }
        return null;
    }
}
