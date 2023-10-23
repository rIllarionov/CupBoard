using Cysharp.Threading.Tasks;

public interface IExitableState : IState
{
    public void OnExit();
}