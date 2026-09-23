using UnityEngine;
public class PatrolEnemySpawner : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private PatrolEnemy patrolPrefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private int maxAliveEnemies = 3;
    [Header("Spawn Config")]
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private Collider2D[] platforms;
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private float minEdgeMargin = 0.5f;
    [SerializeField] private float maxEdgeMargin = 4f;
    [Header("Spawn Safety")]
    [SerializeField] private Transform player;
    [SerializeField] private float minDistanceToPlayer = 4f;
    [SerializeField] private int maxSpawnAttempts = 5;
    private PatrolEnemyService _enemyService;
    private float _timer;
    private void Awake()
    {
        _enemyService = new PatrolEnemyService(patrolPrefab, transform, poolSize);
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) player = playerObject.transform;
        }
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
        if (_enemyService.ActiveCount >= maxAliveEnemies) return;
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Bounds bounds = platforms[Random.Range(0, platforms.Length)].bounds;
            float minX = bounds.min.x + Random.Range(minEdgeMargin, maxEdgeMargin);
            float maxX = bounds.max.x - Random.Range(minEdgeMargin, maxEdgeMargin);
            float spawnY = bounds.max.y + heightOffset;
            bool spawnOnLeft = Random.value > 0.5f;
            Vector2 spawnPos = new Vector2(spawnOnLeft ? minX : maxX, spawnY);
            //Si el extremo elegido esta cerca del jugador, probamos el otro extremo de la plataforma.
            if (!IsFarFromPlayer(spawnPos))
            {
                spawnOnLeft = !spawnOnLeft;
                spawnPos.x = spawnOnLeft ? minX : maxX;
                if (!IsFarFromPlayer(spawnPos)) continue;
            }
            float direction = spawnOnLeft ? 1f : -1f;
            _enemyService.Spawn(spawnPos, direction, minX, maxX);
            return;
        }
    }
    private bool IsFarFromPlayer(Vector2 position)
    {
        if (player == null) return true;
        return Vector2.Distance(position, player.position) >= minDistanceToPlayer;
    }
}
