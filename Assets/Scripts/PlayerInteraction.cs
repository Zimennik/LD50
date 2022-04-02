using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


//This class is responsible for player interaction with IInteractable objects; Interaction implemented with Raycast
//When player looks at an object, it will show a prompt to press E to interact and then when player presses E, it will call the Interact() method of the object
//When player looks away from an object, it will hide the prompt
//If player can Interact with an object, it will call the CursorEnter() method of the object
//If player no longer can't Interact with an object, it will call the CursorExit() method of the object
public class PlayerInteraction : MonoBehaviour
{
    public float raycastDistance = 2f;
    public LayerMask layerMask;
    public TMP_Text prompt;
    public bool canInteract;

    private IInteractable _currentInteractable;

    private Vector3 _playerHandsLocalPos = new Vector3(0, 0.5f, 0.3f);
    private PullableObject _currentObjectInHands;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_currentObjectInHands != null)
            {
                DropObject();
                return;
            }

            if (canInteract && _currentInteractable != null)
            {
                _currentInteractable.Interact();
            }
        }
    }

    public void SetInteract(bool value)
    {
        canInteract = value;
        if (!value)
        {
            prompt.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!canInteract) return;
        RaycastHit hit;
        var t1 = GameManager.Instance.characterController._firstPersonAIO.playerCamera.transform;
        if (Physics.Raycast(t1.position,
                t1.forward, out hit, raycastDistance, layerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.CursorExit();
                }

                _currentInteractable = interactable;
                _currentInteractable.CursorEnter();
                prompt.gameObject.SetActive(true);
                prompt.text = _currentInteractable.CustomText ?? "Press E to interact";
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.CursorExit();
            }

            _currentInteractable = null;
            prompt.gameObject.SetActive(false);
        }
    }

    // When player interact with object, that can be picked up, this object disables its rigidbody
    // also sets its parent to player's hand and move there with DOTween
    public void PickUpObject(PullableObject _pullableObject)
    {
        _currentObjectInHands = _pullableObject;
        SetInteract(false);
        _pullableObject.SetRigidbodyActive(false);
        _pullableObject.transform.SetParent(GameManager.Instance.characterController._firstPersonAIO.transform);
        _pullableObject.transform.localRotation = Quaternion.identity;
        _pullableObject.transform.DOLocalMove(_playerHandsLocalPos, 0.5f);
    }

    // When player drop object, this object enables its rigidbody
    // also sets its parent to null
    public void DropObject()
    {
        if (_currentObjectInHands == null) return;

        SetInteract(true);
        _currentObjectInHands.transform.SetParent(null);
        _currentObjectInHands.SetRigidbodyActive(true);
        _currentObjectInHands.Drop(GameManager.Instance.characterController._firstPersonAIO.playerCamera.transform
            .forward * 50f);
        _currentObjectInHands = null;
    }
}