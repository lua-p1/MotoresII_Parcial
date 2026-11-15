public enum EnemyEvent
{
    Reset,        //el pool entrega el enemigo: volver al estado inicial
    Move,         //empieza a moverse (se notifica una vez, no cada frame)
    FaceLeft,     //cambio de direccion: mirar a la izquierda
    FaceRight,    //cambio de direccion: mirar a la derecha
    AttackPlayer, //toco al player y le hizo damage
    EnemyDie,     //murio
    Pause,        //se pauso el juego
    Resume        //se reanudo el juego
}
