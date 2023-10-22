public interface IEnterebleStateWithContext : IState
{
    public void OnEnter(IContext context);
}