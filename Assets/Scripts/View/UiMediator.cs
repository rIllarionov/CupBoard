using VContainer.Unity;

public class UiMediator : IStartable
{
    private readonly UiController _uiController;
    private readonly StartLevelState _startLevelState;
    private readonly FinishLevelCheckerState _finishLevelCheckerState;

    public UiMediator(UiController uiController, StartLevelState startLevelState, FinishLevelCheckerState finishLevelCheckerState)
    {
        _uiController = uiController;
        _startLevelState = startLevelState;
        _finishLevelCheckerState = finishLevelCheckerState;
    }
    
    public void Start()
    {
        _uiController.OnButtonClick += _finishLevelCheckerState.MoveToStartLevelState;
        _finishLevelCheckerState.OnLevelFinish += _uiController.ShowButton;
        _startLevelState.OnLastLevel += _uiController.ShowTitle;
        _startLevelState.OnLastLevel += this.Unsubscribe;
    }
    private void Unsubscribe()
    {
        _uiController.OnButtonClick -= _finishLevelCheckerState.MoveToStartLevelState;
        _finishLevelCheckerState.OnLevelFinish -= _uiController.ShowButton;
        _startLevelState.OnLastLevel -= _uiController.ShowTitle;
        _startLevelState.OnLastLevel -= this.Unsubscribe;
    }
}