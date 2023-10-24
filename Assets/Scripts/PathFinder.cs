using System.Collections.Generic;

public class PathFinder
{
    private Dictionary<Point, Point> _previousPoint; // В словаре лежат все точки карты и являются ключами,
    // в значении хранится точка из которой мы пришли в конкретную точку

    public List<Point> FindAvailablePoints(Point startPoint)
    {
        _previousPoint = new Dictionary<Point, Point>();

        Queue<Point> queue = new Queue<Point>();

        List<Point> processPoints = new List<Point>(); //проверенные точки
        List<Point> availablePoints = new List<Point>(); //для хранения точек в которые можно дойти

        queue.Enqueue(startPoint);

        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();

            var neighbours = currentPoint.GetNeighbour();

            foreach (var currentNeighbour in neighbours)
            {
                if (currentNeighbour.IsAvailable && !processPoints.Contains(currentNeighbour))
                {
                    queue.Enqueue(currentNeighbour);
                    availablePoints.Add(currentNeighbour);
                    _previousPoint[currentNeighbour] = currentPoint; // Сохраняем предыдущую точку
                }
            }

            processPoints.Add(currentPoint);
        }

        return availablePoints;
    }

    public List<Point> GetPath(Point start, Point finish)
    {
        List<Point> path = new List<Point>();

        if (!_previousPoint.ContainsKey(finish))
        {
            // Нет доступного пути до точки finish
            return path;
        }

        Point current = finish;

        while (current != start)
        {
            path.Add(current);
            current = _previousPoint[current];
        }

        path.Reverse(); // Инвертируем путь
        return path;
    }
}