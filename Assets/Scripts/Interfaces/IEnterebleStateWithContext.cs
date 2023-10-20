public interface IEnterebleStateWithContext : IState
{
    public void OnEnter(IExitableStateWithContext exitableState);
}