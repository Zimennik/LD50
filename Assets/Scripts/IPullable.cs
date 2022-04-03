using UnityEngine;


public interface IPullable
{
    void Pull(Transform target);
    float Mass { get; }
    bool IsAntiMatter { get; }
    Transform Transform { get; }
}