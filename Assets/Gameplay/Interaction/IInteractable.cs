public interface IInteractable
{
    bool IsInteractionActive { get; }

    abstract void Interact();
}
