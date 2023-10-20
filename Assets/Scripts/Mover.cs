using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public void Move(Transform movingTransform, List<Point> points, float moveDuration)
    {
        StartCoroutine(StartMoving(movingTransform, points, moveDuration));
    }

    private IEnumerator StartMoving(Transform movingTransform, List<Point> points, float moveDuration)
    {
        for (int i = 0; i < points.Count; i++)
        {
            var nextPosition = points[i].transform.position;
            StartCoroutine(DoStep(movingTransform, nextPosition, moveDuration / points.Count));

            yield return new WaitForSeconds(moveDuration / points.Count);
        }
    }

    private IEnumerator DoStep(Transform movingTransform, Vector3 nextPosition, float moveDuration)
    {
        movingTransform.DOMove(nextPosition, moveDuration);

        yield return new WaitForSeconds(moveDuration);
    }
}