using DG.Tweening;
using UnityEngine;


//this is a script for a door that can be opened and closed
//when the player interact with it, it will open and close
//animation is implemented with DOTween
public class Door : MonoBehaviour, IInteractable
{
    //[SerializeField] private Collider _interactionCollider;
    [SerializeField] private Transform _doorHolder;
    public bool DisableInteraction => false;
    private bool isOpen = false;

    //implementing IInteractable
    public void Interact()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void CursorEnter()
    {
        Debug.Log("Cursor Enter");
    }

    public void CursorExit()
    {
        Debug.Log("Cursor Exit");
    }

    public string CustomText => $"Press E to {(isOpen ? "close" : "open")} the door";
    public bool IsInteractable => true;

    //open the door
    private void OpenDoor()
    {
        isOpen = true;
        _doorHolder.DOLocalRotate(new Vector3(0, 90, 0), 1f);
    }

    //close the door
    private void CloseDoor()
    {
        isOpen = false;
        _doorHolder.DOLocalRotate(new Vector3(0, 0, 0), 1f);
    }
}