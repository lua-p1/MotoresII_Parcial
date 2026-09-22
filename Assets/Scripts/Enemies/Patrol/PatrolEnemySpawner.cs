using UnityEngine;
public class PatrolEnemySpawner : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private PatrolEnemy patrolPrefab;
    [SerializeField] private int poolSize = 5;
    [Header("Spawn Config")]
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private Collider2D[] platforms;
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private float minEdgeMargin = 0.5f;
    [SerializeField] private float maxEdgeMargin = 4f;

    private PatrolEnemyService _enemyService;
    private float _timer;

    private void Awake()
    {
        _enemyService = new PatrolEnemyService(patrolPrefab, transform, poolSize);
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        Bounds bounds = platforms[Random.Range(0, platforms.Length)].bounds;
        float minX = bounds.min.x + Random.Range(minEdgeMargin, maxEdgeMargin);
        float maxX = bounds.max.x - Random.Range(minEdgeMargin, maxEdgeMargin);
        bool spawnOnLeft = Random.value > 0.5f;
        float spawnX = spawnOnLeft ? minX : maxX;
        float direction = spawnOnLeft ? 1f : -1f;
        float spawnY = bounds.max.y + heightOffset;
        _enemyService.Spawn(new Vector2(spawnX, spawnY), direction, minX, maxX);
    }
}
