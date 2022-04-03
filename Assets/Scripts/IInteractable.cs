//Interface for objects that can be interacted with

public interface IInteractable
{
    void Interact();
    void CursorEnter();
    void CursorExit();

    string CustomText { get; }
    bool IsInteractable { get; }

    bool DisableInteraction { get; }
}