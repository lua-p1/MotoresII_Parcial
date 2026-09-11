using UnityEngine;
public abstract class Controller : MonoBehaviour
{
    protected float _horizontalInput;
    protected int _verticalInput;
    public float GetHorizontalInput() => _horizontalInput;
    public int GetVerticalInput()
    {
        int input = _verticalInput;
        _verticalInput = 0;
        return input;
    }
    public virtual void NotMove() => _horizontalInput = 0;
}
