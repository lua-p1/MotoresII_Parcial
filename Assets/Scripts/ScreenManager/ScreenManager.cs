using System.Collections.Generic;
using UnityEngine;
//RESUMEN: apenas arranca, el ScreenManager le pide al ScreenRoot de la escena
//(campo _root, asignado por inspector) que FABRIQUE la pantalla principal y la
//pushea como base del stack. El ScreenRoot no ES la pantalla: la crea en
//CreateRootScreen() y devuelve un IScreen (ScreenMainMenu en el menu, ScreenGameplay en el nivel).
//
//DETALLE:
//ScreenRoot es una clase abstracta con un metodo abstracto que devuelve un IScreen.
//Push(_root.CreateRootScreen()) le pasa como parametro lo que devuelve el ScreenRoot.
//- Menu: MenuScreenRoot devuelve directamente un ScreenBase (el ScreenMainMenu).
//- Nivel: GameplayScreenRoot recibe un Transform y devuelve un ScreenGameplay armado con el.
//El ScreenRoot de cada escena se asigna por inspector en _root
//(MenuScreenRoot en MainMenu, GameplayScreenRoot en Level1).
//La ventaja -> el ScreenManager no se toca mas; cada escena arma su ScreenRoot como quiera.
//La unica condicion: devolver siempre un IScreen. (PD: ScreenBase es un IScreen :D)
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }
    private Stack<IScreen> _screenStack = new Stack<IScreen>();
    [SerializeField] private ScreenRoot _root;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (_root == null)
        {
            Debug.LogError("ScreenManager: falta asignar el ScreenRoot de la escena.");
            return;
        }
        Push(_root.CreateRootScreen());
    }
    public void Pop()
    {
        if (_screenStack.Count == 1) return; // si solo queda el root (siempre el de abajo de todo), no lo saco
        var oldScreen = _screenStack.Pop();  // lo saca del stack y te lo devuelve
        oldScreen.Release();// lo libera (polimorfismo: en ScreenBase es Destroy(gameObject))
        _screenStack.Peek().Activate();// Peek solo MIRA el nuevo de arriba, no lo saca, y lo activa
    }
    public void Push(string resource)
    {
        var screen = Instantiate(Resources.Load<ScreenBase>(resource));
        Push(screen);
    }
    public void Push(IScreen screen)
    {
        if (_screenStack.Count > 0)            // si hay alguna pantalla abierta...
        {
            _screenStack.Peek().Deactivate();  // ...desactivo la de arriba de todo (sin sacarla del stack)
        }
        _screenStack.Push(screen);             // meto la nueva screen arriba del stack
        screen.Activate();                     // la activo (polimorfismo: override propio o el de ScreenBase)
    }
    public bool StackContainsType<T>()
    {
        foreach (var screen in _screenStack)
            if (screen is T) return true;
        return false;
    }
}
