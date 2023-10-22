using System.Collections.Generic;

public class PathFinderStateContext : IContext
{
    public Point StartPoint { get; private set; }
    public List<Point> Path { get; private set; }
    public PathFinderStateContext(Point startPoint, List<Point> path)
    {
        StartPoint = startPoint;
        Path = path;
    }
    
}