using UnityEngine;
using UnityEngine.InputSystem;

namespace Rogue {

public class Input : MonoBehaviour
{
    private Gui.ContextManager mManager = new Gui.ContextManager();

    public Gui.ContextManager Contexts => mManager;

    public Vector2 Pointer { get; private set; }

    public Vector2 Camera { get; private set; }

    #region @@@ ACTIONS @@@

    public void OnClick()
    {
        mManager.Enqueue("click");
    }

    public void OnPress()
    {
        mManager.Enqueue("press");
    }

    public void OnRelease()
    {
        mManager.Enqueue("release");
    }

    public void OnPoint(InputValue value)
    {
        Pointer = value.Get<Vector2>();
    }

    public void OnCamera(InputValue value)
    {
        Camera = value.Get<Vector2>();
    }

    #endregion

    private void Start()
    {
        
    }

    private void Update()
    {
        mManager.Update(Time.deltaTime);
    }
}

}
