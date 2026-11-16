//Interfaz para estrategias de movimientos de los enemigos
public interface IMovementEnemy
{
    float Direction { get; }
    void Move();
}
