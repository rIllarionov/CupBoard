public class CatchStartPointStateContext : IContext
{
    public Point StartPoint { get; private set; }
    public CatchStartPointStateContext(Point startPoint)
    {
        StartPoint = startPoint;
    }
    
}