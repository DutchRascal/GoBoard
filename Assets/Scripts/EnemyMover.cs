using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovemementType
{
    Stationary,
    Patrol,
    Spinner
}

public class EnemyMover : Mover
{
    public Vector3 directionToMove = new Vector3(0f, 0f, Board.spacing);
    public MovemementType movementType = MovemementType.Stationary;
    public float standTime = 1f;
    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void MoveOneTurn()
    {
        switch (movementType)
        {
            case MovemementType.Patrol:
                Patrol();
                break;
            case MovemementType.Stationary:
                Stand();
                break;
            case MovemementType.Spinner:
                Spin();
                break;
        }
    }

    void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        Vector3 startPos = new Vector3(CurrentNode.Coordinate.x, 0f, CurrentNode.Coordinate.y);
        Vector3 newDest = startPos + transform.TransformVector(directionToMove);
        Vector3 nextDest = startPos + transform.TransformVector(directionToMove * 2f);
        Move(newDest, 0f);
        while (isMoving)
        {
            yield return null;
        }
        if (m_board)
        {
            Node newDestNode = m_board.FindNoteAt(newDest);
            Node nextDestNode = m_board.FindNoteAt(nextDest);
            if (nextDestNode == null || !newDestNode.LinkedNodes.Contains(nextDestNode))
            {
                destination = startPos;
                FaceDestination();
                yield return new WaitForSeconds(rotateTime);
            }
        }
        base.finishMovementEvent.Invoke();
    }

    void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(standTime);
        base.finishMovementEvent.Invoke();
    }

    void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        Vector3 localForward = new Vector3(0f, 0f, Board.spacing);
        destination = transform.TransformVector(localForward * -1f) + transform.position;
        FaceDestination();
        yield return new WaitForSeconds(rotateTime);
        base.finishMovementEvent.Invoke();
    }
}
