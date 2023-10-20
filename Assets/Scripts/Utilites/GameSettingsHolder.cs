using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsHolder
{
    private Queue<string> _settings;

    public int ChipsCount { get; private set; }
    public int PointsCount { get; private set; }
    public List<int> StartChipsPosition { get; private set; } = new();
    public List<int> WinChipsPosition { get; private set; } = new();
    public List<Vector2> PointsCoordinates { get; private set; }
    public List<Vector2> Connections { get; private set; }
    
    private int _connectionsCount;

    public void Initialize(string[] settings)
    {
        _settings =
            ArrayConverter.ConvertArrayToQueue(settings);

        Decrypt();
    }

    private void Decrypt()
    {
        ChipsCount = Convert.ToInt32(_settings.Dequeue());
        PointsCount = Convert.ToInt32(_settings.Dequeue());

        SavePointsCoordinates();
        SaveChipsPosition();
        SaveConnections();
    }

    private void SaveConnections()
    {
        _connectionsCount = Convert.ToInt32(_settings.Dequeue());
        Connections = GetVectorsFromQueue(_connectionsCount);
    }

    private void SavePointsCoordinates()
    {
        PointsCoordinates = GetVectorsFromQueue(PointsCount);
    }

    private List<Vector2> GetVectorsFromQueue(int vectorsCount)
    {
        List<Vector2> vectorList = new List<Vector2>();

        for (int i = 0; i < vectorsCount; i++)
        {
            string[] values = _settings.Dequeue().Split(',');

            var x = Convert.ToInt32(values[0]);
            var y = Convert.ToInt32(values[1]);

            vectorList.Add(new Vector2(x, y));
        }

        return vectorList;
    }

    private void SaveChipsPosition()
    {
        var startChipsPosition = _settings.Dequeue().Split(",");
        var winChipsPosition = _settings.Dequeue().Split(",");

        for (int i = 0; i < ChipsCount; i++)
        {
            StartChipsPosition.Add(Convert.ToInt32(startChipsPosition[i]));
            WinChipsPosition.Add(Convert.ToInt32(winChipsPosition[i]));
        }
    }
}