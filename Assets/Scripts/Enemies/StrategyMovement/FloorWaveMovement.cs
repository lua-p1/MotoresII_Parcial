using UnityEngine;
//Estrategia de movimiento del Ghost.
//Que hace el fantasma:
//1) Cruza la pantalla de un lado al otro y se da vuelta en los bordes.
//2) Mientras avanza sube y baja un poquito (ondula), como si flotara.
//3) Cada 2 a 4 segundos elige un piso y se desliza suave hasta esa altura.
//4) Cuando elige piso, la mayoria de las veces elige el piso donde esta el player.
public class FloorWaveMovement : IMovementEnemy
{
    //El fantasma que se mueve (su transform)
    private Transform _target;
    //El player: lo uso solo para saber en que piso esta
    private Transform _player;
    //La altura a la que vuela el fantasma en cada piso (la calcula el GhostSpawner)
    private float[] _floorHeights;
    //Bordes izquierdo y derecho de la pantalla: ahi se da vuelta
    private float _minX, _maxX;
    //Velocidad horizontal promedio
    private float _horizontalSpeed;
    //Cuanto puede cambiar la velocidad (0.25 = entre 75% y 125% de la velocidad)
    private float _speedVariation;
    //Que tan alto sube y baja al ondular
    private float _floatAmplitude;
    //Que tan rapido sube y baja al ondular
    private float _floatFrequency;
    //Tiempo minimo y maximo entre cambios de piso
    private float _minFloorChangeTime;
    private float _maxFloorChangeTime;
    //Probabilidad de elegir el piso del player (0.6 = 60%)
    private float _chanceToPickPlayerFloor;
    //Cuanto tarda en llegar a la altura del piso nuevo (mas alto = mas lento y suave)
    private float _smoothTime;
    //Hacia donde va: 1 derecha, -1 izquierda
    private float _direction;
    //Altura actual del fantasma sin contar la onda
    private float _currentBaseY;
    //Altura del piso al que quiere llegar
    private float _targetBaseY;
    //Variable que necesita SmoothDamp para calcular el movimiento suave (no se toca a mano)
    private float _yVelocity;
    //Cuanto tiempo falta para elegir un piso nuevo
    private float _floorTimer;
    //Numero al azar para que cada fantasma ondule distinto y no todos iguales
    private float _randomSeed;
    //La lee el EnemyController para avisarle al Model hacia donde mira (y la View hace el flip)
    public float Direction => _direction;

    //Constructor: guarda todos los datos que le pasa el GhostEnemy en Initialize
    //y prepara el estado inicial del movimiento.
    public FloorWaveMovement(Transform target, Vector2 spawnPos, float initialDirection,
        float minX, float maxX, float[] floorHeights, Transform player,
        float horizontalSpeed, float speedVariation, float floatAmplitude, float floatFrequency,
        float minFloorChangeTime, float maxFloorChangeTime, float chanceToPickPlayerFloor, float smoothTime)
    {
        _target = target;
        //Mathf.Sign deja solo el signo: 1 o -1
        _direction = Mathf.Sign(initialDirection);
        _minX = minX;
        _maxX = maxX;
        _floorHeights = floorHeights;
        _player = player;
        _horizontalSpeed = horizontalSpeed;
        _speedVariation = speedVariation;
        _floatAmplitude = floatAmplitude;
        _floatFrequency = floatFrequency;
        _minFloorChangeTime = minFloorChangeTime;
        _maxFloorChangeTime = maxFloorChangeTime;
        _chanceToPickPlayerFloor = chanceToPickPlayerFloor;
        _smoothTime = smoothTime;
        //Arranca en la altura donde aparecio (ya es la altura de un piso)
        _currentBaseY = spawnPos.y;
        _targetBaseY = spawnPos.y;
        //El primer cambio de piso pasa en un tiempo al azar entre el minimo y el maximo
        _floorTimer = Random.Range(_minFloorChangeTime, _maxFloorChangeTime);
        _randomSeed = Random.Range(0f, 100f);
    }

    //Lo llama el EnemyController en cada frame (desde Tick).
    //Primero revisa si toca cambiar de piso, despues calcula la nueva X y la nueva Y,
    //y por ultimo mueve al fantasma a esa posicion.
    public void Move()
    {
        HandleFloorTimer();
        float newX = MoveHorizontal();
        float newY = MoveVertical();
        _target.position = new Vector2(newX, newY);
    }

    //Calcula la nueva X.
    //Avanza hacia _direction y se da vuelta cuando llega a un borde de la pantalla.
    //La velocidad no es fija: sube y baja un poco para que no se vea como un robot.
    //
    //Como funciona speedVariation (ejemplo con horizontalSpeed = 1.2 y speedVariation = 0.25):
    //speedWave es un seno: va cambiando suave entre -1 y 1 con el tiempo.
    //speed = horizontalSpeed * (1 + speedWave * speedVariation)
    //  speedWave =  1 -> 1 + 0.25 = 1.25 (125%) -> 1.2 * 1.25 = 1.5 (lo mas rapido)
    //  speedWave =  0 -> 1 + 0    = 1    (100%) -> 1.2 * 1    = 1.2 (velocidad normal)
    //  speedWave = -1 -> 1 - 0.25 = 0.75 (75%)  -> 1.2 * 0.75 = 0.9 (lo mas lento)
    private float MoveHorizontal()
    {
        //Seno lento que va de -1 a 1 (la mitad de rapido que la onda vertical)
        float speedWave = Mathf.Sin((Time.time + _randomSeed) * _floatFrequency * 0.5f);
        //Velocidad final: la velocidad normal mas o menos un porcentaje (speedVariation)
        float speed = _horizontalSpeed * (1f + speedWave * _speedVariation);
        //Nueva X: donde estaba + direccion * velocidad * tiempo del frame
        float newX = _target.position.x + (_direction * speed * Time.deltaTime);
        //Si llega al borde izquierdo yendo a la izquierda, gira a la derecha
        if (newX <= _minX && _direction < 0)
            _direction = 1f;
        //Si llega al borde derecho yendo a la derecha, gira a la izquierda
        else if (newX >= _maxX && _direction > 0)
            _direction = -1f;
        return newX;
    }

    //Calcula la nueva Y.
    //Se acerca de a poco a la altura del piso elegido y le suma la onda para que flote.
    private float MoveVertical()
    {
        //SmoothDamp acerca _currentBaseY a _targetBaseY de forma suave:
        //arranca despacio, acelera en el medio y frena al llegar.
        //Tarda siempre mas o menos el mismo tiempo en llegar (lo marca _smoothTime),
        //sin importar la distancia. Por eso, cuanto mas lejos esta el piso, mas acelera.
        //Ejemplo con smoothTime = 0.4 (llega en ~1 segundo):
        //  1 piso  (~3 unidades) -> velocidad maxima ~5 por segundo
        //  2 pisos (~6 unidades) -> velocidad maxima ~10 por segundo
        //  3 pisos (~9 unidades) -> velocidad maxima ~15 por segundo
        //Si subo smoothTime tarda mas y va mas lento. Si lo bajo tarda menos y va mas rapido.
        //_yVelocity guarda la velocidad vertical de ese momento (la actualiza SmoothDamp solo).
        _currentBaseY = Mathf.SmoothDamp(_currentBaseY, _targetBaseY, ref _yVelocity, _smoothTime);
        //Onda: seno que va de -1 a 1, multiplicado por la amplitud (cuanto sube y baja)
        float floatOffset = Mathf.Sin((Time.time + _randomSeed) * _floatFrequency) * _floatAmplitude;
        //Altura final: la altura del piso + la onda
        return _currentBaseY + floatOffset;
    }

    //Cuenta el tiempo para el proximo cambio de piso.
    //Cuando llega a 0: elige un piso nuevo y vuelve a arrancar el timer con un tiempo al azar.
    private void HandleFloorTimer()
    {
        //Le resto el tiempo que paso desde el frame anterior
        _floorTimer -= Time.deltaTime;
        //Si todavia queda tiempo, no hago nada
        if (_floorTimer > 0f) return;
        //Se termino el tiempo: nuevo tiempo al azar y nuevo piso al que ir
        _floorTimer = Random.Range(_minFloorChangeTime, _maxFloorChangeTime);
        _targetBaseY = _floorHeights[ChooseFloor()];
    }

    //Decide a que piso ir. Devuelve la posicion del piso en el array _floorHeights.
    //Random.value da un numero entre 0 y 1: si es menor que la chance, va al piso del player.
    //Si no, elige cualquier piso al azar (puede volver a salir el del player).
    //Si no hay player, siempre elige al azar.
    private int ChooseFloor()
    {
        if (_player != null && Random.value < _chanceToPickPlayerFloor)
            return GetPlayerFloor();
        return Random.Range(0, _floorHeights.Length);
    }

    //Averigua en que piso esta el player.
    //Recorre todos los pisos y se queda con el que tiene la altura mas cercana a la del player.
    private int GetPlayerFloor()
    {
        //Arranco suponiendo que el mas cercano es el primer piso
        int closestFloor = 0;
        float closestDistance = Mathf.Abs(_player.position.y - _floorHeights[0]);
        //Comparo con el resto de los pisos
        for (int i = 1; i < _floorHeights.Length; i++)
        {
            //Mathf.Abs deja la distancia siempre positiva (no importa si esta arriba o abajo)
            float distance = Mathf.Abs(_player.position.y - _floorHeights[i]);
            //Si este piso esta mas cerca, pasa a ser el mas cercano
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFloor = i;
            }
        }
        return closestFloor;
    }
}
