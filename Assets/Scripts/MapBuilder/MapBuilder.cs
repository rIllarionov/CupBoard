using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private LineDrawer _lineDrawer;

    [SerializeField] private Transform _mapPrefab;

    [SerializeField] private Point _pointPrefab;
    [SerializeField] private Chip _chipPrefab;

    [SerializeField] private Vector3 _minimapOffset;
    [SerializeField] private Vector3 _minimapSize;

    [SerializeField] private List<Sprite> _chipSprites;

    private GameSettingsHolder _gameSettingsHolder;
    public List<Chip> MapChips { get; } = new();
    public List<Point> MapPoints { get; } = new();
    public List<Chip> MinimapChips { get; } = new();

    private List<Point> _minimapPoints = new();

    private const int _indexOffset = 1;

    private Transform _map;
    private Transform _miniMap;


    public void Build(GameSettingsHolder gameSettingsHolder)
    {
        _gameSettingsHolder = gameSettingsHolder;

        CreateMaps();
        BuildMap(_map, MapPoints, _gameSettingsHolder.StartChipsPosition, MapChips);
        BuildMap(_miniMap, _minimapPoints, _gameSettingsHolder.WinChipsPosition, MinimapChips);

        ReplaceMinimap();
    }

    public void DestroyMap()
    {
        Destroy(_miniMap.gameObject);
        Destroy(_map.gameObject);

        MinimapChips.Clear();
        MapChips.Clear();
        MapPoints.Clear();
        _minimapPoints.Clear();
    }

    private void CreateMaps()
    {
        _map = Instantiate(_mapPrefab);
        _miniMap = Instantiate(_mapPrefab);
    }

    private void BuildMap(Transform mapRoot, List<Point> mapPoints, List<int> chipsPosition, List<Chip> chips)
    {
        SetPoints(mapRoot, mapPoints);
        SetPaths(mapRoot, mapPoints);
        SetChips(mapPoints, chipsPosition, chips);
    }

    private void SetPoints(Transform mapRoot, List<Point> mapPoints)
    {
        foreach (var coordinate in _gameSettingsHolder.PointsCoordinates)
        {
            var mapPoint = Instantiate(_pointPrefab, mapRoot, false);
            mapPoint.transform.position = coordinate;
            mapPoints.Add(mapPoint);
        }
    }

    private void SetPaths(Transform mapRoot, List<Point> mapPoints)
    {
        for (int i = 0; i < _gameSettingsHolder.Connections.Count; i++)
        {
            var currentConnection = _gameSettingsHolder.Connections[i];

            var firstPoint = mapPoints[(int)(currentConnection.x - _indexOffset)];
            var secondPoint = mapPoints[(int)(currentConnection.y - _indexOffset)];

            firstPoint.SetNeighbour(secondPoint);
            secondPoint.SetNeighbour(firstPoint);

            _lineDrawer.DrawLine(mapRoot, firstPoint.transform.position, secondPoint.transform.position);
        }
    }

    private void SetChips(List<Point> mapPoints, List<int> chipsPositions, List<Chip> chips)
    {
        for (int i = 0; i < _gameSettingsHolder.ChipsCount; i++)
        {
            var currentPoint = chipsPositions[i];

            var currentChip = Instantiate(_chipPrefab, mapPoints[currentPoint - _indexOffset].transform, false);
            mapPoints[currentPoint - _indexOffset].SetChip(currentChip);

            currentChip.SetSprite(_chipSprites[i]);
            chips.Add(currentChip);

            //указываем корректные индексы точек
            currentChip.CurrentPosition = currentPoint - _indexOffset;
        }
    }

    private void ReplaceMinimap()
    {
        _miniMap.position = _minimapOffset;
        _miniMap.localScale = _minimapSize;
    }
}