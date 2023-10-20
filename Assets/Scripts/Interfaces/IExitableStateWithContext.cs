public interface IExitableStateWithContext : IState
{
    public IExitableStateWithContext OnExit();
}