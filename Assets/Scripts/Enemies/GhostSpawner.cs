using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private GhostEnemy ghostPrefab;
    [SerializeField] private int poolSize = 5;

    [Header("Spawn Config")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnPaddingX = 1.2f; // Distancia fuera de la pantalla para spawnear

    [Header("Vertical Boundaries (Plataformas 1 a 3)")]
    [Tooltip("Altura mínima (sobre la Plataforma 0 / SolidGround)")]
    [SerializeField] private float platform1MinY = -1.5f;

    [Tooltip("Altura máxima (Límite de Plataforma 3)")]
    [SerializeField] private float platform3MaxY = 3.5f;

    [Header("Max Ghosts Limit")]
    [SerializeField] private int maxActiveGhosts = 2;

    private List<GhostEnemy> _pool = new List<GhostEnemy>();
    private Camera _mainCamera;
    private float _timer;

    private float _screenMinX;
    private float _screenMaxX;

    private void Awake()
    {
        _mainCamera = Camera.main;
        InitializePool();
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

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GhostEnemy ghost = Instantiate(ghostPrefab, transform);
            ghost.gameObject.SetActive(false);
            _pool.Add(ghost);
        }
    }

    private int GetActiveGhostCount()
    {
        int count = 0;
        foreach (var ghost in _pool)
        {
            if (ghost.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
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

        GhostEnemy newGhost = Instantiate(ghostPrefab, transform);
        newGhost.gameObject.SetActive(false);
        _pool.Add(newGhost);
        return newGhost;
    }

    private void CalculateHorizontalBounds()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

        _screenMinX = bottomLeft.x;
        _screenMaxX = topRight.x;
    }

    public void TrySpawnGhost()
    {
        // Regla: Si hay 2 o más fantasmas activos, NO spawnea ninguno más
        if (GetActiveGhostCount() >= maxActiveGhosts) return;

        GhostEnemy ghost = GetPooledGhost();
        if (ghost == null) return;

        // Elegir aleatoriamente si aparece por la Izquierda (true) o Derecha (false)
        bool spawnOnLeft = Random.value > 0.5f;

        float spawnX = spawnOnLeft ? _screenMinX - spawnPaddingX : _screenMaxX + spawnPaddingX;
        float direction = spawnOnLeft ? 1f : -1f;

        // Y dentro del rango de las plataformas 1 a 3 (evitando plataforma 0 / SolidGround)
        float spawnY = Random.Range(platform1MinY, platform3MaxY);

        ghost.gameObject.SetActive(true);
        ghost.Initialize(new Vector2(spawnX, spawnY), direction, _screenMinX, _screenMaxX, platform1MinY, platform3MaxY);
    }
}