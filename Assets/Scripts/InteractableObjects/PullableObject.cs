using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PullableObject : AffectedByBlackHole, IPullable, IPickupable, IInteractable
{
    [SerializeField] private GameObject _antimatterObject;
    [SerializeField] private GameObject _regularObject;

    public Transform pivotPoint;

    private float defaultForce = 1f;
    public float Mass => rb.mass;

    public bool IsInteractable { get; private set; }
    public bool IsAntiMatter { get; private set; }

    public Transform Transform => transform;

    public bool canBePickedUp;

    public bool disableInteraction = false;
    public bool DisableInteraction => disableInteraction;


    public void Update()
    {
        if (transform.position.y < -10)
        {
            GameManager.Instance.blackHoleController.RemoveObjectFromList(this);
            Destroy(this.gameObject);
        }
    }

// Apply force to pull this object to center of target
//The closer the object is to the center of the target, the stronger the pull
    public void Pull(Transform target)
    {
        if (GameManager.Instance.IsCutscenePlaying || GameManager.Instance.isGameOver) return;
        var force = (target.position - transform.position).normalized * defaultForce * blackHoleController.currentSize;
        rb.AddForce(force);
    }

    public void PickUp()
    {
        if (disableInteraction) return;

        IsPickedUp = true;
        IsInteractable = false;
        GameManager.Instance.characterController._playerInteraction.PickUpObject(this);
    }

    public void Drop(Vector3 force)
    {
        IsPickedUp = false;
        IsInteractable = true;
        rb.AddForce(force);
    }

    public bool IsPickedUp { get; private set; }

    public void Interact()
    {
        if (DisableInteraction) return;
        PickUp();
    }

    public void CursorEnter()
    {
    }

    public void CursorExit()
    {
    }

//replace all renderer materials this object and its child to anti-matter material
//for every meshrenderer in this object add anti-matter particle system
//set particle system shape to mesh of this object
    public void ConvertToAntiMatter()
    {
        if (IsAntiMatter) return;
        IsAntiMatter = true;

        GameManager.Instance._audioSource.PlayOneShot(GameManager.Instance._antimatterClip);

        _antimatterObject.SetActive(true);
        _regularObject.SetActive(false);
    }

    public void SetRigidbodyActive(bool value)
    {
        rb.isKinematic = !value;
        collider.enabled = value;
    }

    public string CustomText => $"Press E to pick up";
}