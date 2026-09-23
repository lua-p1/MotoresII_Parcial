public abstract class EnemyService<T> where T : EnemyBase
{
    private Pool<T> _pool;
    private Factory<T> _factory;
    public int ActiveCount { get; private set; }
    protected EnemyService(Factory<T> factory, int size)
    {
        _factory = factory;
        _pool = new Pool<T>(CreateEnemy, TurnOn, TurnOff, size);
    }
    private T CreateEnemy()
    {
        T enemy = _factory.CreateObject();
        enemy.SetReturnToPoolCallback(e => ReturnToPool((T)e));
        return enemy;
    }
    protected T GetFromPool()
    {
        ActiveCount++;
        return _pool.GetObject();
    }
    private void TurnOn(T enemy)
    {
        enemy.gameObject.SetActive(true);
    }
    private void TurnOff(T enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    private void ReturnToPool(T enemy)
    {
        ActiveCount--;
        _pool.ReturnObject(enemy);
    }
}
