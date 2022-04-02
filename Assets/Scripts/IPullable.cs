using UnityEngine;


public interface IPullable
{
    void Pull(Transform target);
    float Mass { get; }
}