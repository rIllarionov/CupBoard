using Cysharp.Threading.Tasks;

public interface IEnterableState : IState
{
    public void OnEnter();
}