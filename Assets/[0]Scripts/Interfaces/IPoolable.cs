public interface IPoolable
{
    void OnActivate(object argument = default);
    void OnDeactivate(object argument = default);
}