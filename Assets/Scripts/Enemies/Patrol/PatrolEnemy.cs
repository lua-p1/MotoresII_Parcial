using UnityEngine;
//Enemigo Patrol que hereda de EnemyBase
public class PatrolEnemy : EnemyBase
{
    [Header("Patrol Movement Config")]
    [SerializeField] private float speed = 2f;
    //Metodo propio para inicializarlo que le cargo la estrategia de Movimiento y lo reinicio(todos los enemigos se reinician iguales con el metodo del padre)
    //Este metodo se llama en PatrolEnemyService 
    public void Initialize(Vector2 spawnPos, float initialDirection, float minX, float maxX)
    {
        IMovementEnemy movement = new PatrolMovement(transform, initialDirection, minX, maxX, speed);
        ResetEnemy(spawnPos, movement);
    }
}
