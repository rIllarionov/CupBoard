using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderState : MonoBehaviour, IEnterebleStateWithContext, IExitableStateWithContext
{
    [SerializeField] private MapBuilder _mapBuilder;
    
    private readonly PathFinder _pathFinder = new();
    private readonly HighLighter _highLighter = new();

    private StateMachine _stateMachine;

    private Action<List<ILightable>, bool> _turnAllLights;

    private List<Point> _availablePaths;
    private Point _startPoint;
    private List<Point> _path;

    private bool _isActive;


    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(IContext context)
    {
        _turnAllLights += _highLighter.SwitchLights;
        ReadContext(context);
        FindPathsFrom(_startPoint);

        _isActive = true;
    }

    public IContext OnExit()
    {
        _isActive = false;
        _turnAllLights -= _highLighter.SwitchLights;
        return new PathFinderStateContext(_startPoint, _path);
    }

    private void Update()
    {
        if (_isActive && Input.GetMouseButtonDown(0))
        {
            _turnAllLights?.Invoke(new List<ILightable>(_mapBuilder.MapPoints), false);

            var finishPoint = GetFinishPoint();

            if (finishPoint != null)
            {
                _path = _pathFinder.GetPath(_startPoint, finishPoint);
                _stateMachine.Enter<MovingState>();
            }
            else
            {
                _stateMachine.Enter<CatchStartPointState>();
            }
        }
    }

    private Point GetFinishPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit info))
        {
            if (info.collider.CompareTag("Point"))
            {
                var point = info.collider.GetComponent<Point>();

                //вынести в отдельный метод?
                if (_availablePaths.Contains(point))
                {
                    return point;
                }
            }
        }

        return null;
    }

    private void ReadContext(IContext context)
    {
        var catchStartPointState = (CatchStartPointStateContext)context;
        _startPoint = catchStartPointState.StartPoint;
    }

    private void FindPathsFrom(Point startPoint)
    {
        _availablePaths = _pathFinder.FindAvailablePoints(startPoint);
        _turnAllLights?.Invoke(new List<ILightable>(_availablePaths), true);
    }
}