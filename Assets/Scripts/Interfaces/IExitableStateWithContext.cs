
public interface IExitableStateWithContext : IState
{
    public IContext OnExit();
}