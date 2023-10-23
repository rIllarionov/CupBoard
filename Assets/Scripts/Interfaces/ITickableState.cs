using Cysharp.Threading.Tasks;

public interface ITickableState
{ 
    public void OnTick();
}