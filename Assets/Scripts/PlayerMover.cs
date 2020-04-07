using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{


    PlayerCompass m_playerCompass;



    void UpdateBoard()
    {
        if (m_board)
        {
            m_board.UpdatePlayerNode();
        }
    }
}
