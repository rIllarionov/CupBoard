using VContainer.Unity;

public class GameManager : ITickable, IStartable
{
    private StateMachine _stateMachine;
    private readonly UiMediator _uiMediator;

    public GameManager(StartLevelState startLevelState,
        CatchStartPointState catchStartPointState,
        PathFinderState pathFinderState,
        MovingState movingState,
        FinishLevelCheckerState finishLevelCheckerState,
        UiMediator uiMediator)
    {
        _stateMachine =
            new StateMachine(startLevelState,
                catchStartPointState,
                pathFinderState,
                movingState,
                finishLevelCheckerState);
        _uiMediator = uiMediator;
    }

    public void Start()
    {
        _uiMediator.Initialize();
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