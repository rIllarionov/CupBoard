using VContainer.Unity;

public class GameManager : ITickable
{
    private StateMachine _stateMachine;

    public GameManager(StartLevelState startLevelState,
        CatchStartPointState catchStartPointState,
        PathFinderState pathFinderState,
        MovingState movingState,
        FinishLevelCheckerState finishLevelCheckerState)
    {
        _stateMachine =
            new StateMachine(startLevelState,
                catchStartPointState,
                pathFinderState,
                movingState,
                finishLevelCheckerState);
    }

    public void Start()
    {
        _stateMachine.Enter<StartLevelState>();
    }

    public void Tick()
    {
        if (_stateMachine.CurrentState is ITickableState tickableState)
        {
            tickableState.OnTick();
        }
    }
}