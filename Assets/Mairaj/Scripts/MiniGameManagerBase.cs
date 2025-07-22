using UnityEngine;

public abstract class MiniGameManagerBase : MonoBehaviour
{
    public abstract void InitializeGame();
    public abstract void EndGame();

    private void Start()
    {
        InitializeGame();
    }

    protected virtual void OnEnable()
    {
        RegisterCallbacks();
    }

    protected virtual void OnDisable()
    {
        UnregisterCallbacks();
    }

    protected abstract void RegisterCallbacks();
    protected abstract void UnregisterCallbacks();
}
