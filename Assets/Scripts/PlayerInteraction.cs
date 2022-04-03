using DG.Tweening;
using TMPro;
using UnityEngine;


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

    private Vector3 _playerHandsLocalPos = new Vector3(-0.3f, 0.5f, 0.5f);
    private PullableObject _currentObjectInHands;


    private Vector3 _convertLocalPosition = new Vector3(0.3f, -0.19f, 0.19f);
    private bool isConverterInHands = false;


    private Tween _currentTween;

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

            if (interactable.DisableInteraction) return;
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
        if (_currentTween != null) _currentTween.Kill();
        _currentObjectInHands = _pullableObject;
        SetInteract(false);
        _pullableObject.SetRigidbodyActive(false);
        _pullableObject.transform.SetParent(GameManager.Instance.characterController._firstPersonAIO.transform);
        _pullableObject.transform.localRotation = Quaternion.identity;
        _currentTween = _pullableObject.transform.DOLocalMove(_playerHandsLocalPos, 0.5f);
    }

    // When player drop object, this object enables its rigidbody
    // also sets its parent to null
    public void DropObject()
    {
        if (_currentObjectInHands == null) return;

        if (_currentTween != null) _currentTween.Kill();

        SetInteract(true);
        _currentObjectInHands.SetRigidbodyActive(true);
        _currentObjectInHands.transform.SetParent(null);
        _currentObjectInHands.Drop((GameManager.Instance.characterController._firstPersonAIO.playerCamera.transform
            .forward + Vector3.up * 0.3f) * 500f * _currentObjectInHands.Mass);
        _currentObjectInHands = null;
    }

    //Set player's camera as parent of the converter
    //Smooth move converter to _convertLocalPosition with DOTween
    //Enable Converter script
    public void PickUpConverter(Converter converterGameObject)
    {
        isConverterInHands = true;
        converterGameObject.isInHands = true;
        converterGameObject.TurnCollider(false);

        converterGameObject.transform.SetParent(GameManager.Instance.characterController._firstPersonAIO.playerCamera
            .transform);
        converterGameObject.transform.localRotation = Quaternion.identity;
        converterGameObject.transform.localPosition = _convertLocalPosition;
        converterGameObject.transform.DOLocalMove(_convertLocalPosition, 0.5f);

        GameManager.Instance.PlayConverterCutscene();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}