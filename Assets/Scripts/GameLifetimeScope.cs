using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private UiController _uiController;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);

        builder.Register<StartLevelState>(Lifetime.Singleton);
        builder.Register<CatchStartPointState>(Lifetime.Singleton);
        builder.Register<PathFinderState>(Lifetime.Singleton);
        builder.Register<MovingState>(Lifetime.Singleton);
        builder.Register<FinishLevelCheckerState>(Lifetime.Singleton);
        builder.Register<MoveAnimator>(Lifetime.Singleton);
        builder.Register<HighLighter>(Lifetime.Singleton);
        builder.Register<PathFinder>(Lifetime.Singleton);
        builder.Register<UiMediator>(Lifetime.Singleton);

        builder.RegisterInstance(_mapBuilder);
        builder.RegisterInstance(_uiController);

        builder.RegisterEntryPoint<GameManager>();
    }
}