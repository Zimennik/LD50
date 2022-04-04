using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] public AudioSource _AudioSource;

    private float growSpeed = 0.035f;
    public float maxSize = 100.0f;
    public float startSize = 0.2f;

    [SerializeField] private AudioClip _absorbClip;

    public float currentSize => transform.localScale.x;
    private bool firstAntiMatterConsumed = false;

    private float growCoef = 1;

    private List<PullableObject> _pullableObjects = new List<PullableObject>();

    public float totalAntimatterMass = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * startSize;
        //find every pullable object on scene and add them to list
        foreach (var pullableObject in FindObjectsOfType<PullableObject>())
        {
            _pullableObjects.Add(pullableObject);
        }

        GameManager.Instance.SetObjectsLeft(_pullableObjects.Count);
    }

    // Update is called once per frame
    void Update()
    {
        _AudioSource.volume = 0.1f + (currentSize / 6) * 0.9f;
        if (GameManager.Instance.IsCutscenePlaying) return;

        growCoef += Time.deltaTime * 0.03f;

        transform.localScale += Vector3.one * growSpeed * growCoef * Time.deltaTime;
        if (transform.localScale.x > maxSize)
        {
            transform.localScale = Vector3.one * maxSize;
        }

        //volume base on size
    }

    public void AddMass(PullableObject pullable)
    {
        RemoveObjectFromList(pullable);

        if (pullable.IsAntiMatter && !firstAntiMatterConsumed)
        {
            firstAntiMatterConsumed = true;
            GameManager.Instance.PlayAntiMatterConsumeCutscene();
        }

        if (pullable.IsAntiMatter)
        {
            totalAntimatterMass += pullable.Mass;
        }

        var newsize = Mathf.Clamp(transform.localScale.x + (pullable.Mass / 2 * (pullable.IsAntiMatter ? -1.5f : 1)),
            startSize, maxSize);
        transform.DOScale(newsize, 0.5f);

        _AudioSource.PlayOneShot(_absorbClip);
    }

    public void RemoveObjectFromList(PullableObject pullable)
    {
        if (_pullableObjects.Contains(pullable))
        {
            _pullableObjects.Remove(pullable);

            GameManager.Instance.SetObjectsLeft(_pullableObjects.Count);

            if (_pullableObjects.Count == 0)
                GameManager.Instance.PlayEndingCutscene();
        }
    }
}