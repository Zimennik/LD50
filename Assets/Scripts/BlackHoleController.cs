using DG.Tweening;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    private float growSpeed = 0.01f;
    public float maxSize = 100.0f;
    public float startSize = 0.1f;

    public float currentSize => transform.localScale.x;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * startSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
        if (transform.localScale.x > maxSize)
        {
            transform.localScale = Vector3.one * maxSize;
        }
    }

    public void AddMass(IPullable pullable)
    {
        var newsize = Mathf.Clamp(transform.localScale.x + (pullable.Mass * (pullable.IsAntiMatter ? -1 : 1)),
            startSize, maxSize);
        transform.DOScale(newsize, 0.5f);
    }
}