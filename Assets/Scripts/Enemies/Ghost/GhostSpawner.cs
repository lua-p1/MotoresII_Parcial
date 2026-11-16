using UnityEngine;
//Encargado de spawnear Ghosts.
//Aparecen desde afuera de la pantalla, a la altura de un piso al azar,
//y vuelan entre los bordes de la pantalla cambiando de piso.
public class GhostSpawner : MonoBehaviour , IPauseable
{
    [Header("Pool Setup")]
    [SerializeField] private GhostEnemy ghostPrefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private int maxAliveEnemies = 3;
    [Header("Spawn Config")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnPaddingX = 1.2f;
    [Header("Floors (Pisos)")]
    //Plataformas de cada piso donde puede volar el fantasma
    [SerializeField] private Collider2D[] floors;
    //A cuanta altura sobre cada plataforma vuela el fantasma
    [SerializeField] private float floorHeightOffset = 1f;
    [Header("Player")]
    //El player: si queda vacio se busca por tag en el Awake
    [SerializeField] private Transform player;
    [Header("Spawn Safety")]
    //Distancia minima al player para poder aparecer
    [SerializeField] private float minDistanceToPlayer = 4f;
    //Cuantas veces prueba un lugar antes de rendirse hasta el proximo spawn
    [SerializeField] private int maxSpawnAttempts = 5;
    private GhostEnemyService _enemyService;
    private Camera _mainCamera;
    private float _timer;
    private float _screenMinX;
    private float _screenMaxX;
    //Altura de vuelo de cada piso (se calcula en Start a partir de floors)
    private float[] _floorHeights;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _enemyService = new GhostEnemyService(ghostPrefab, transform, poolSize);
        //Si no se asigno el player en el Inspector, lo busco por tag (igual que el PatrolEnemySpawner)
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) player = playerObject.transform;
        }
    }
    private void Start()
    {
        CalculateHorizontalBounds();
        CalculateFloorHeights();
    }
    private void Update()
    {
        if (!GameManager.IsPlaying)
            return;
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            TrySpawnGhost();
        }
    }
    //Tomamos la esquina inferior izquierda y superior derecha de la camara para saber los bordes horizontales de la pantalla.
    //El fantasma aparece afuera de esos bordes (con spawnPaddingX) y despues los usa como limites para darse vuelta.
    private void CalculateHorizontalBounds()
    {
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));
        _screenMinX = bottomLeft.x;
        _screenMaxX = topRight.x;
    }
    //Calculo a que altura vuela el fantasma en cada piso:
    //la parte de arriba de la plataforma (bounds.max.y) mas floorHeightOffset.
    private void CalculateFloorHeights()
    {
        _floorHeights = new float[floors.Length];
        for (int i = 0; i < floors.Length; i++)
            _floorHeights[i] = floors[i].bounds.max.y + floorHeightOffset;
    }
    private void TrySpawnGhost()
    {
        if (_enemyService.ActiveCount >= maxAliveEnemies) return;
        //Sin pisos cargados no hay donde volar
        if (_floorHeights.Length == 0) return;
        //Los dos lugares posibles en X: afuera de la pantalla a la izquierda o a la derecha
        float leftX = _screenMinX - spawnPaddingX;
        float rightX = _screenMaxX + spawnPaddingX;
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            //Elijo un piso al azar y un costado al azar
            float spawnY = _floorHeights[Random.Range(0, _floorHeights.Length)];
            bool spawnOnLeft = Random.value > 0.5f;
            Vector2 spawnPos = new Vector2(spawnOnLeft ? leftX : rightX, spawnY);
            //Si el costado elegido esta cerca del player, pruebo el otro costado.
            //Si los dos estan cerca, paso al siguiente intento (otro piso al azar).
            if (!IsFarFromPlayer(spawnPos))
            {
                spawnOnLeft = !spawnOnLeft;
                spawnPos.x = spawnOnLeft ? leftX : rightX;
                if (!IsFarFromPlayer(spawnPos)) continue;
            }
            //Si sale por la izquierda va a la derecha (1), si sale por la derecha va a la izquierda (-1)
            float direction = spawnOnLeft ? 1f : -1f;
            //Llamo al Spawn del service: saca un fantasma del pool (solo instancia uno nuevo si el pool esta vacio) y lo inicializa
            _enemyService.Spawn(spawnPos, direction, _screenMinX, _screenMaxX, _floorHeights, player);
            return;
        }
    }
    private bool IsFarFromPlayer(Vector2 position)
    {
        if (player == null) return true;
        return Vector2.Distance(position, player.position) >= minDistanceToPlayer;
    }
    public void Pause()
    {
        enabled = false;   // frena el timer de spawn
    }
    public void Resume()
    {
        enabled = true;
    }
}