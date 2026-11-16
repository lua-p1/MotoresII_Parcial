using UnityEngine;
//GhostEnemy hereda de EnemyBase.
//Se mueve con la estrategia FloorWaveMovement: cruza ondulando y cambia de piso,
//con mas chance de ir al piso del player.
public class GhostEnemy : EnemyBase
{
    [Header("Horizontal Movement Config")]
    //Velocidad horizontal promedio
    [SerializeField] private float horizontalSpeed = 3f;
    //Cuanto varia la velocidad (0.25 = entre 75% y 125%). Menor a 1 para que nunca vaya para atras.
    [Range(0f, 0.9f)]
    [SerializeField] private float speedVariation = 0.25f;
    [Header("Smooth Floating (Flotacion Constante)")]
    //Que tan alto sube y baja al ondular
    [SerializeField] private float floatAmplitude = 0.35f;
    //Que tan rapido sube y baja al ondular
    [SerializeField] private float floatFrequency = 2.5f;
    [Header("Floor Change (Cambio de Piso)")]
    //Tiempo minimo y maximo entre cambios de piso (se elige uno al azar entre los dos)
    [SerializeField] private float minFloorChangeTime = 2f;
    [SerializeField] private float maxFloorChangeTime = 4f;
    //Probabilidad de elegir el piso del player (0 = nunca, 1 = siempre)
    [Range(0f, 1f)]
    [SerializeField] private float chanceToPickPlayerFloor = 0.6f;
    //Cuanto tarda en llegar al piso nuevo (mas alto = mas lento y suave)
    [SerializeField] private float smoothTime = 0.7f;

    //Metodo propio para inicializarse con su estrategia de movimiento.
    //Lo llama GhostEnemyService cuando saca el fantasma del pool.
    //Crea el FloorWaveMovement con los datos que le llegan del spawner (posicion, direccion,
    //bordes de pantalla, alturas de los pisos y el player) mas su propia config del Inspector,
    //y despues llama a ResetEnemy del padre (igual que todos los enemigos).
    public void Initialize(Vector2 spawnPos, float initialDirection, float minX, float maxX,
        float[] floorHeights, Transform player)
    {
        IMovementEnemy movement = new FloorWaveMovement(transform, spawnPos, initialDirection,
            minX, maxX, floorHeights, player,
            horizontalSpeed, speedVariation, floatAmplitude, floatFrequency,
            minFloorChangeTime, maxFloorChangeTime, chanceToPickPlayerFloor, smoothTime);
        ResetEnemy(spawnPos, movement);
    }
}