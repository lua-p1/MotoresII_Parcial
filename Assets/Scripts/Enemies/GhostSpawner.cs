using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private GhostEnemy ghostPrefab;
    [SerializeField] private int poolSize = 10;

    [Header("Spawn Config")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float screenPadding = 1f; // Distancia fuera de pantalla para spawnear/despawnear
    [SerializeField] private float topMargin = 1f;    // Margen superior respecto a la pantalla
    [SerializeField] private float bottomMargin = 1f; // Margen inferior respecto a la pantalla

    private List<GhostEnemy> _pool = new List<GhostEnemy>();
    private Camera _mainCamera;
    private float _timer;

    private float _leftEdge;
    private float _rightEdge;
    private float _bottomEdge;
    private float _topEdge;

    private void Awake()
    {
        _mainCamera = Camera.main;
        InitializePool();
    }

    private void Start()
    {
        CalculateScreenBounds();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            SpawnGhost();
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GhostEnemy ghost = Instantiate(ghostPrefab, transform);
            ghost.gameObject.SetActive(false);
            _pool.Add(ghost);
        }
    }

    private GhostEnemy GetPooledGhost()
    {
        foreach (var ghost in _pool)
        {
            if (!ghost.gameObject.activeInHierarchy)
            {
                return ghost;
            }
        }

        // Si el pool se queda corto, crea un elemento extra
        GhostEnemy newGhost = Instantiate(ghostPrefab, transform);
        newGhost.gameObject.SetActive(false);
        _pool.Add(newGhost);
        return newGhost;
    }

    private void CalculateScreenBounds()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

        _leftEdge = bottomLeft.x;
        _rightEdge = topRight.x;
        _bottomEdge = bottomLeft.y + bottomMargin;
        _topEdge = topRight.y - topMargin;
    }

    public void SpawnGhost()
    {
        GhostEnemy ghost = GetPooledGhost();
        if (ghost == null) return;

        // Elegir aleatoriamente si aparece por la Izquierda (0) o Derecha (1)
        bool spawnOnLeft = Random.value > 0.5f;

        float spawnX = spawnOnLeft ? _leftEdge - screenPadding : _rightEdge + screenPadding;
        float targetX = spawnOnLeft ? _rightEdge + screenPadding : _leftEdge - screenPadding;
        float direction = spawnOnLeft ? 1f : -1f;

        float spawnY = Random.Range(_bottomEdge, _topEdge);

        ghost.gameObject.SetActive(true);
        ghost.Initialize(new Vector2(spawnX, spawnY), direction, targetX, _bottomEdge, _topEdge);
    }
}