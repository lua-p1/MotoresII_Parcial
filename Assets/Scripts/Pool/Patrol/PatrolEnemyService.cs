using UnityEngine;
//Servicio de Patrols: hereda de EnemyService<PatrolEnemy>, que vincula el pool con el factory y cuenta los activos.
//Este hijo le pasa el PatrolEnemyFactory al padre y agrega Spawn con los parametros propios del Patrol.
public class PatrolEnemyService : EnemyService<PatrolEnemy>
{
    // El hijo recibe el prefab y el parent porque los necesita para construir su PatrolEnemyFactory.
    // Una vez creada la fabrica, se la pasa al constructor del padre junto con el tamano del pool.
    // El padre no necesita conocer el prefab ni el parent, solamente recibe una fabrica ya construida
    // para administrar la creacion y reutilizacion de enemigos.
    public PatrolEnemyService(PatrolEnemy prefab, Transform parent, int size)
        : base(new PatrolEnemyFactory(prefab, parent), size)
    {
    }
    //Metodo exclusivo de este hijo que se llama en PatrolEnemySpawner
    //Devuelve un enemigo del pool y lo inicializa cargandole su IMovementEnemy y aplicando ResetEnemy
    public PatrolEnemy Spawn(Vector2 position, float direction, float minX, float maxX)
    {
        PatrolEnemy enemy = GetFromPool();
        enemy.Initialize(position, direction, minX, maxX);
        return enemy;
    }
}
