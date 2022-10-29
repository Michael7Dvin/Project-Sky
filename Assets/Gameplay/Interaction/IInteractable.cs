public interface IInteractable
{
    bool IsInteractionAllowed { get; }

    abstract void Interact();
}
