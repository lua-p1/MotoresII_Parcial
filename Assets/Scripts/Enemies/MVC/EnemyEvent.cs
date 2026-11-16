//Observer del MVC del enemigo:
//- El unico Observable es el EnemyModel: es el que notifica los eventos con NotifyObservers.
//- Los Observers son EnemyView y EnemyBase: se suscriben al Model en el Awake del EnemyBase.
//- El EnemyController no observa ni notifica: llama a metodos del Model, y el Model notifica.
//Todos los observers suscriptos al Model reciben TODOS los eventos.
//Cada uno reacciona solo a los que tiene en su diccionario.
public enum EnemyEvent
{
    Reset,        //el pool entrega el enemigo: volver al estado inicial -> solo reacciona EnemyView.
    Move,         //empieza a moverse (se notifica una vez, no cada frame) -> solo reacciona EnemyView.
    FaceLeft,     //cambio de direccion: mirar a la izquierda -> solo reacciona EnemyView.
    FaceRight,    //cambio de direccion: mirar a la derecha -> solo reacciona EnemyView.
    AttackPlayer, //toco al player e intento hacerle damage (si es invulnerable no le hace) -> solo reacciona EnemyView.
    EnemyDie,     //murio -> reaccionan EnemyView y EnemyBase.
    Pause,        //se pauso el juego -> solo reacciona EnemyView.
    Resume        //se reanudo el juego -> solo reacciona EnemyView.
}
