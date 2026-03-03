using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallToHoleController : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private BallGridOccupancy occupancy;
    [SerializeField] private Transform holeJumpPoint;

    public event Action<IBall> OnBallDroppedIntoHole;

    private void Awake()
    {
        inputHandler.OnClickedBall += RequestSendToHole;
    }

    private void OnDestroy()
    {
        inputHandler.OnClickedBall -= RequestSendToHole;
    }

    private void RequestSendToHole(IBall ball)
    {
        if (ball.IsConnected == false)
        {
            if (!CanDequeue(ball)) return;
            ball.FlyToHole(holeJumpPoint.position, () => OnBallReachedHole(ball));
            occupancy.Unregister(ball.Cell.x, ball.Cell.y);
            GameManager.Instance.DecreaseQueueBallCount();
            BallGridOccupancy.Instance.Collapse();
        }

        else
        {
            var other = ball.ConnectedBall;
            if (other == null) return;

            if (!CanDequeue(ball) || !CanDequeue(other)) return;
            SendBallsTogether(new List<IBall> { ball, other });
        }
    }


    private bool CanDequeue(IBall ball)
    {
        int row = ball.Cell.x;
        int col = ball.Cell.y;

        for (int r = 0; r < row; r++)
        {
            if (occupancy.GetAt(r, col) != null)
                return false;
        }

        return true;
    }


    private void SendBallsTogether(List<IBall> balls)
    {
        StartCoroutine(SendConnectedRoutine(balls));
    }

    private IEnumerator SendConnectedRoutine(List<IBall> balls)
    {
        foreach (var ball in balls)
        {
            occupancy.Unregister(ball.Cell.x, ball.Cell.y);
            GameManager.Instance.DecreaseQueueBallCount();
        }

        foreach (var ball in balls)
        {
            ball.ConnectedBall = null;
        }

        occupancy.Collapse();

        foreach (var ball in balls)
        {
            ball.FlyToHole(holeJumpPoint.position, () => OnBallReachedHole(ball));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnBallReachedHole(IBall ball)
    {
        OnBallDroppedIntoHole?.Invoke(ball);
    }
}