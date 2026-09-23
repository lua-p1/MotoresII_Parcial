using UnityEngine;
public class GhostSpawner : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private GhostEnemy ghostPrefab;
    [SerializeField] private int poolSize = 5;
    [Header("Spawn Config")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnPaddingX = 1.2f;
    [Header("Vertical Boundaries (Plataformas 1 a 3)")]
    [SerializeField] private float platform1MinY = -1.5f;
    [SerializeField] private float platform3MaxY = 3.5f;
    private GhostEnemyService _enemyService;
    private Camera _mainCamera;
    private float _timer;
    private float _screenMinX;
    private float _screenMaxX;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _enemyService = new GhostEnemyService(ghostPrefab, transform, poolSize);
    }
    private void Start()
    {
        CalculateHorizontalBounds();
    }
    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            TrySpawnGhost();
        }
    }
    //Tomamos la esquina inferior izquierda y superior derecha para saber el rango donde puede aparecer el enemigo.
    private void CalculateHorizontalBounds()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));
        _screenMinX = bottomLeft.x;
        _screenMaxX = topRight.x;
    }
    private void TrySpawnGhost()
    {
        bool spawnOnLeft = Random.value > 0.5f;
        float spawnX = spawnOnLeft ? _screenMinX - spawnPaddingX : _screenMaxX + spawnPaddingX;
        float direction = spawnOnLeft ? 1f : -1f;
        float spawnY = Random.Range(platform1MinY, platform3MaxY);
        _enemyService.Spawn(new Vector2(spawnX, spawnY), direction, _screenMinX, _screenMaxX, platform1MinY, platform3MaxY);
    }
}