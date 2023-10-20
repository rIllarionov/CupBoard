using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRendererPrefab;

    private List<LineRenderer> _lines = new();


    public void DrawLine(Transform parent, params Vector3[] points)
    {
        var lineRenderer = Instantiate(_lineRendererPrefab, parent, false);

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        _lines.Add(lineRenderer);
    }

    //возможно этот метод лишний
    public void ClearLines()
    {
        foreach (var line in _lines)
        {
            Destroy(line.gameObject);
        }

        _lines.Clear();
    }
}