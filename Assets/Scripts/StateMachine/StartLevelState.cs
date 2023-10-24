using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class StartLevelState : IEnterableState, IExitableState
{
    public Action OnLastLevel;

    private readonly MapBuilder _mapBuilder;
    private GameSettingsHolder _gameSettingsHolder;
    private StateMachine _stateMachine;

    private List<string[]> _levelsSettings;
    private int _levelNumber;

    public StartLevelState(MapBuilder mapBuilder)
    {
        _mapBuilder = mapBuilder;
    }

    public void Initialize(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        ReadLevelsSettings();
    }

    public void OnExit()
    {
        _levelNumber++;
    }

    public void OnEnter()
    {
        if (_levelNumber > 0)
        {
            _mapBuilder.DestroyMap();
        }

        if (_levelNumber == _levelsSettings.Count)
        {
            _mapBuilder.DestroyMap();
            OnLastLevel?.Invoke();
            return;
        }

        PrepareLevelSettings();
        BuildAndEnterAsync().Forget();
    }

    private async UniTask BuildAndEnterAsync()
    {
        await _mapBuilder.Build(_gameSettingsHolder);
        _stateMachine.Enter<CatchStartPointState>();
    }

    private void ReadLevelsSettings()
    {
        _levelsSettings = new List<string[]>
        {
            FileRider.ReadFile("Assets/GameSettings/test.txt"),
            FileRider.ReadFile("Assets/GameSettings/test1.txt")
        };
    }

    private void PrepareLevelSettings()
    {
        var settings = _levelsSettings[_levelNumber];

        _gameSettingsHolder = new GameSettingsHolder();
        _gameSettingsHolder.Initialize(settings);
    }
}