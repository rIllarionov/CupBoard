using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    private const int _millisecondsDelay = 100;

    private Transform _map;
    private Transform _miniMap;


    public async UniTask Build(GameSettingsHolder gameSettingsHolder)
    {
        _gameSettingsHolder = gameSettingsHolder;

        CreateMaps();
        await BuildMap(_miniMap, _minimapPoints, _gameSettingsHolder.WinChipsPosition, MinimapChips);
        ReplaceMinimap();
        await BuildMap(_map, MapPoints, _gameSettingsHolder.StartChipsPosition, MapChips); 
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

    private async UniTask BuildMap(Transform mapRoot, List<Point> mapPoints, List<int> chipsPosition, List<Chip> chips)
    {
        await SetPoints(mapRoot, mapPoints);
        await SetPaths(mapRoot, mapPoints);
        await SetChips(mapPoints, chipsPosition, chips);
    }

    private async UniTask SetPoints(Transform mapRoot, List<Point> mapPoints)
    {
        foreach (var coordinate in _gameSettingsHolder.PointsCoordinates)
        {
            var mapPoint = Instantiate(_pointPrefab, mapRoot, false);
            mapPoint.transform.position = coordinate;
            mapPoints.Add(mapPoint);

            await UniTask.Delay(_millisecondsDelay);
        }
    }

    private async UniTask SetPaths(Transform mapRoot, List<Point> mapPoints)
    {
        for (int i = 0; i < _gameSettingsHolder.Connections.Count; i++)
        {
            var currentConnection = _gameSettingsHolder.Connections[i];

            var firstPoint = mapPoints[(int)(currentConnection.x - _indexOffset)];
            var secondPoint = mapPoints[(int)(currentConnection.y - _indexOffset)];

            firstPoint.SetNeighbour(secondPoint);
            secondPoint.SetNeighbour(firstPoint);

            _lineDrawer.DrawLine(mapRoot, firstPoint.transform.position, secondPoint.transform.position);
            
            await UniTask.Delay(_millisecondsDelay);
        }
    }

    private async UniTask SetChips(List<Point> mapPoints, List<int> chipsPositions, List<Chip> chips)
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
            
            await UniTask.Delay(_millisecondsDelay);
        }
    }

    private void ReplaceMinimap()
    {
        var duration = 1f;

        _miniMap.DOMove(_minimapOffset, duration);
        _miniMap.DOScale(_minimapSize, duration);
    }
}