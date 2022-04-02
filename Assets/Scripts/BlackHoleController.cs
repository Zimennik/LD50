using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public float growSpeed = 0.1f;
    public float maxSize = 100.0f;
    public float startSize = 0.1f;


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
}