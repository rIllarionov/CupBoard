using Cysharp.Threading.Tasks;

public interface IEnterebleStateWithContext : IState
{
    public void OnEnter(IContext context);
}