using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PullableObject : AffectedByBlackHole, IPullable, IPickupable, IInteractable
{
    private float defaultForce = 1f;
    public float Mass => rb.mass;

    public bool IsInteractable { get; private set; }

    public bool IsAntiMatter = false;

    // Apply force to pull this object to center of target
    //The closer the object is to the center of the target, the stronger the pull
    public void Pull(Transform target)
    {
        var force = (target.position - transform.position).normalized * defaultForce * blackHoleController.currentSize;
        rb.AddForce(force);
    }

    public void PickUp()
    {
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
        PickUp();
        ConvertToAntiMatter();
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

        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (var material in renderer.materials)
            {
                renderer.materials = new Material[] { GameManager.Instance.antiMatterMaterial };
            }
        }

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            var particleSystem = Instantiate(GameManager.Instance.antiMatterParticleSystem, renderer.transform);
            particleSystem.transform.localPosition = Vector3.zero;
            particleSystem.transform.localRotation = Quaternion.identity;
            particleSystem.transform.localScale = Vector3.one;
            particleSystem.transform.parent = null;

            var psr = particleSystem.GetComponent<ParticleSystemRenderer>();
            psr.renderMode = ParticleSystemRenderMode.Mesh;
            var mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
            mesh.SetTriangles(new int[] { }, 1);
            psr.mesh = mesh;
            particleSystem.Play();
        }
    }

    public void SetRigidbodyActive(bool value)
    {
        rb.isKinematic = !value;
        collider.enabled = value;
    }

    public string CustomText => $"Pick up {name}";
}