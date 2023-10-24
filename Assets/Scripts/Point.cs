using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour, ILightable
{
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _hightlightColor;
    [SerializeField] private SpriteRenderer _renderer;

    public bool IsAvailable { get; private set; }
    public Chip Chip { get; private set; }

    private List<Point> _neighbourPoints = new();

    public void SwitchLight(bool state)
    {
        _renderer.color = state ? _hightlightColor : _normalColor;
    }

    public void SetNeighbour(Point neighbourPoint)
    {
        _neighbourPoints.Add(neighbourPoint);
    }

    public List<Point> GetNeighbour()
    {
        return _neighbourPoints;
    }

    public void SetChip(Chip chip)
    {
        Chip = chip;
        ChangeAvailable(false);
    }

    public void ClearPoint()
    {
        Chip = null;
        ChangeAvailable(true);
    }

    private void Awake()
    {
        ChangeAvailable(true);
    }

    private void ChangeAvailable(bool state)
    {
        IsAvailable = state;
    }
}