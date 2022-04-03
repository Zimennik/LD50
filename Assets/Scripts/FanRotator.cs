using UnityEngine;

public class FanRotator : MonoBehaviour
{
    [SerializeField] private Transform fan;
    public float speed;
    public bool isRotating = true;

    // Update is called once per frame
    void Update()
    {
        if (fan == null) return;
        fan.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}