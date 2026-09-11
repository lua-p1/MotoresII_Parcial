using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Contoller : MonoBehaviour
{
    protected Vector2 _moveDir;
    public abstract Vector2 GetMovementInput();

}
