using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class MovingState : IEnterebleStateWithContext
{
    private readonly MoveAnimator _moveAnimator;
    private readonly MapBuilder _mapBuilder;

    private Point _startPoint;
    private List<Point> _path;

    private StateMachine _stateMachine;

    public MovingState(MapBuilder mapBuilder, MoveAnimator moveAnimator)
    {
        _mapBuilder = mapBuilder;
        _moveAnimator = moveAnimator;
    }

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(IContext context)
    {
        ReadContext(context); 
        Move();
    }

    private void ReadContext(IContext context)
    {
        var pathFinderContext = (PathFinderStateContext)context;
        _startPoint = pathFinderContext.StartPoint;
        _path = pathFinderContext.Path;
    }

    private async UniTask Move()
    {
        var chipTransform = _startPoint.Chip.transform;
        await _moveAnimator.StartAnimation(chipTransform, _path);

        ReplaceChip();
        _stateMachine.Enter<FinishLevelCheckerState>();
    }

    private void ReplaceChip()
    {
        var currentChip = _startPoint.Chip;
        _path[^1].SetChip(currentChip);
        currentChip.CurrentPosition = _mapBuilder.MapPoints.IndexOf(_path[^1]);
        _startPoint.ClearPoint();
    }
}