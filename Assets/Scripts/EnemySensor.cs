using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    Node m_nodeTosearch;
    Board m_board;
    bool m_foundPlayer = false;

    public Vector3 directionToSearch = new Vector3(0f, 0f, 2f);
    public bool FoundPlayer { get => m_foundPlayer; }

    // Start is called before the first frame update
    void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void UpdateSensor()
    {
        Vector3 wordSpacePositionToSearch = transform.TransformVector(directionToSearch) + transform.position;
        if (m_board)
        {
            m_nodeTosearch = m_board.FindNoteAt(wordSpacePositionToSearch);
            if (m_nodeTosearch == m_board.PlayerNode)
            {
                m_foundPlayer = true;
            }
        }
    }
}
