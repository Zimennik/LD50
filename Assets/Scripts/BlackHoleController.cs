using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    private float growSpeed = 0.03f;
    public float maxSize = 100.0f;
    public float startSize = 0.1f;

    public float currentSize => transform.localScale.x;
    private bool firstAntiMatterConsumed = false;


    private float growCoef = 1;

    private List<PullableObject> _pullableObjects = new List<PullableObject>();

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * startSize;
        //find every pullable object on scene and add them to list
        foreach (var pullableObject in FindObjectsOfType<PullableObject>())
        {
            _pullableObjects.Add(pullableObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsCutscenePlaying) return;

        growCoef += Time.deltaTime * 0.02f;

        transform.localScale += Vector3.one * growSpeed * growCoef * Time.deltaTime;
        if (transform.localScale.x > maxSize)
        {
            transform.localScale = Vector3.one * maxSize;
        }
    }

    public void AddMass(PullableObject pullable)
    {
        RemoveObjectFromList(pullable);

        if (pullable.IsAntiMatter && !firstAntiMatterConsumed)
        {
            firstAntiMatterConsumed = true;
            GameManager.Instance.PlayAntiMatterConsumeCutscene();
        }

        var newsize = Mathf.Clamp(transform.localScale.x + (pullable.Mass * (pullable.IsAntiMatter ? -1 : 1)),
            startSize, maxSize);
        transform.DOScale(newsize, 0.5f);
    }

    public void RemoveObjectFromList(PullableObject pullable)
    {
        if (_pullableObjects.Contains(pullable))
        {
            _pullableObjects.Remove(pullable);

            if (_pullableObjects.Count == 0)
                GameManager.Instance.PlayEndingCutscene();
        }
    }
}