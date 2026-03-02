using System;
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
        if (ball == null) return;
        // İsteğe bağlı: connected kuralı burada kontrol et
        // if (!CanDequeue(ball)) return;


        ball.JumpTo(holeJumpPoint.position, () => OnBallReachedHole(ball));
        occupancy.UnRegisterBall(ball);
        BallGridOccupancy.Instance.Collapse();
    }

    private void OnBallReachedHole(IBall ball)
    {
        OnBallDroppedIntoHole?.Invoke(ball);
    }
}