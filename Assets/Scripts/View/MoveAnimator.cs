using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MoveAnimator
{
    private readonly float _moveDuration = 1f;

    public async UniTask StartAnimation(Transform movingTransform, List<Point> points)
    {
        await Move(movingTransform, points);
    }

    private async UniTask Move(Transform movingTransform, List<Point> points)
    {
        var oneStepDuration = _moveDuration / points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            var nextPosition = points[i].transform.position;
            await movingTransform.DOMove(nextPosition, oneStepDuration).ToUniTask();
        }
    }
}