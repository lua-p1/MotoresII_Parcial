public enum EnemyEvent
{
    Reset,        //el pool entrega el enemigo: volver al estado inicial -> se suscribe solo EnemyView.
    Move,         //empieza a moverse (se notifica una vez, no cada frame) -> se suscribe solo EnemyView.
    FaceLeft,     //cambio de direccion: mirar a la izquierda -> se suscribe solo EnemyView.
    FaceRight,    //cambio de direccion: mirar a la derecha -> se suscribe solo EnemyView.
    AttackPlayer, //toco al player y le hizo damage -> se suscribe solo EnemyView
    EnemyDie,     //murio -> se suscribe solo EnemyView y EnemyBase.
    Pause,        //se pauso el juego -> se suscribe solo EnemyView
    Resume        //se reanudo el juego -> se suscribe solo EnemyView
}
