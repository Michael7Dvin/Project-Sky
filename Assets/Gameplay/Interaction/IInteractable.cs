public interface IInteractable
{
    bool IsActive { get; }

    abstract void Interact();
}
