public abstract class EnemyService<T> where T : EnemyBase
{
    private Pool<T> _pool;
    private Factory<T> _factory;
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
    public void ReturnToPool(T enemy)
    {
        _pool.ReturnObject(enemy);
    }
}
