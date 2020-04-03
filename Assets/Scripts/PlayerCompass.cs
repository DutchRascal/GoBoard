using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour
{
    Board m_board;
    List<GameObject> m_arrows = new List<GameObject>();

    public GameObject arrowPrefab;
    public float
        scale = 1f,
        startDistance = 0.25f,
        endDistance = 0.5f,
        moveTime = 1f,
        delay = 0f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    private void Awake()
    {
        m_board = FindObjectOfType<Board>().GetComponent<Board>();
        SetUpArrows();
        MoveArrows();
    }

    void SetUpArrows()
    {
        if (!arrowPrefab)
        {
            Debug.LogWarning("PLAYERCOMPASS SetupArrows ERROR: Missing arrow prefab!");
            return;
        }

        foreach (Vector2 dir in Board.directions)
        {
            Vector3 dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            GameObject arrowInstance = Instantiate(arrowPrefab, transform.position + dirVector * startDistance, rotation);
            arrowInstance.transform.localScale = new Vector3(scale, scale, scale);
            arrowInstance.transform.parent = transform;
            m_arrows.Add(arrowInstance);
        }
    }

    void MoveArrow(GameObject arrowInstance)
    {
        iTween.MoveBy(arrowInstance, iTween.Hash(
            "z", endDistance,
            "looptype", iTween.LoopType.loop,
            "time", moveTime,
            "easetype", easeType
            ));
    }

    void MoveArrows()
    {
        foreach (GameObject arrow in m_arrows)
        {
            MoveArrow(arrow);
        }
    }
}
