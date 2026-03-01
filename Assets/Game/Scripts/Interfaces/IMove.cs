using System;
using UnityEngine;

public interface IMove
{
    void MoveTo(Vector3 target);
    void JumpTo(Vector3 target, Action onComplete);
}