using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CatchStartPointState : IEnterableState, IExitableStateWithContext, ITickableState
{
    private readonly MapBuilder _mapBuilder;
    private readonly HighLighter _highLighter;

    private StateMachine _stateMachine;
    private bool _isActive;
    private Point _startPoint;

    public CatchStartPointState(MapBuilder mapBuilder, HighLighter highLighter)
    {
        _mapBuilder = mapBuilder;
        _highLighter = highLighter;
    }

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter()
    {
        _isActive = true;
    }

    public IContext OnExit()
    {
        _isActive = false;
        return new CatchStartPointStateContext(_startPoint);
    }

    public void OnTick()
    {
        if (_isActive && Input.GetMouseButtonUp(0))
        {
            _highLighter.SwitchLights(new List<ILightable>(_mapBuilder.MapPoints), false);

            var startPoint = GetStartPoint();

            if (startPoint != null)
            {
                _startPoint = startPoint;

                _highLighter.SwitchLights(new List<ILightable> { _startPoint.Chip }, true); 
                _stateMachine.Enter<PathFinderState>();
            }
        }
    }

    private Point GetStartPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit info))
        {
            if (info.collider.CompareTag("Point"))
            {
                var point = info.collider.GetComponent<Point>();
                if (!point.IsAvailable)
                {
                    return point;
                }
            }
        }

        return null;
    }
}