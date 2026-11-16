using UnityEngine;
//Servicio de Tiradores: hereda de EnemyService<ShooterEnemy>, que vincula el pool con el factory y cuenta los activos.
//Este hijo le pasa el ShooterEnemyFactory al padre y agrega Spawn con los parametros propios del Tirador.
public class ShooterEnemyService : EnemyService<ShooterEnemy>
{
    // El hijo recibe el prefab y el parent porque los necesita para construir su ShooterEnemyFactory.
    // Una vez creada la fabrica, se la pasa al constructor del padre junto con el tamano del pool.
    // El padre no necesita conocer el prefab ni el parent, solamente recibe una fabrica ya construida
    // para administrar la creacion y reutilizacion de enemigos.
    public ShooterEnemyService(ShooterEnemy prefab, Transform parent, int size)
        : base(new ShooterEnemyFactory(prefab, parent), size)
    {
    }
    //Metodo exclusivo de este hijo que se llama en ShooterEnemySpawner
    //Devuelve un Tirador del pool y lo inicializa con el Initialize que hereda de PatrolEnemy
    //(le carga su PatrolMovement y aplica ResetEnemy). Su FSM arranca en Patrol con el OnEnable del ShooterEnemy.
    public ShooterEnemy Spawn(Vector2 position, float direction, float minX, float maxX)
    {
        ShooterEnemy enemy = GetFromPool();
        enemy.Initialize(position, direction, minX, maxX);
        return enemy;
    }
}
