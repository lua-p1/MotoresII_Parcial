using UnityEngine;
//Servicio de Ghost: hereda de EnemyService<GhostEnemy>, que vincula el pool con el factory y cuenta los activos.
//Este hijo le pasa el GhostEnemyFactory al padre y agrega Spawn con los parametros propios del Ghost.
public class GhostEnemyService : EnemyService<GhostEnemy>
{
    // El hijo recibe el prefab y el parent porque los necesita para construir su GhostEnemyFactory.
    // Una vez creada la fabrica, se la pasa al constructor del padre junto con el tamano del pool.
    // El padre no necesita conocer el prefab ni el parent, solamente recibe una fabrica ya construida
    // para administrar la creacion y reutilizacion de enemigos.
    public GhostEnemyService(GhostEnemy prefab, Transform parent, int size): base(new GhostEnemyFactory(prefab, parent), size) { }
    //Metodo exclusivo de este hijo que se llama en GhostSpawner
    //Devuelve un enemigo del pool y lo inicializa cargandole su IMovementEnemy y aplicando ResetEnemy
    public GhostEnemy Spawn(Vector2 position, float direction, float minX, float maxX, float minY, float maxY)
    {
        GhostEnemy enemy = GetFromPool();
        enemy.Initialize(position, direction, minX, maxX, minY, maxY);
        return enemy;
    }
}
