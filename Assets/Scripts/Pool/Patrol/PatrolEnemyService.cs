using UnityEngine;
//Clase que se encarga de vincular el pool con el factoy hereda de una clase Generica llamada EnemyService
public class PatrolEnemyService : EnemyService<PatrolEnemy>
{
    //Uso el constructor del padre
    public PatrolEnemyService(PatrolEnemy prefab, Transform parent, int size): base(new PatrolEnemyFactory(prefab, parent), size)
    { 
    }
    //Metodo exclusivo de este hijo que se llama en PatrolEnemySpawner
    //Deuelve un enemigo del pool y lo incializa cargandole su IMovementEnemy y aplicnado ResetEnemy
    public PatrolEnemy Spawn(Vector2 position, float direction, float minX, float maxX)
    {
        PatrolEnemy enemy = GetFromPool();
        enemy.Initialize(position, direction, minX, maxX);
        return enemy;
    }
}
