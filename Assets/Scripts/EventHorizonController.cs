using DG.Tweening;
using UnityEngine;


//if IPullable object touches this object, IPullable will be destroyed and send his mass parameter to BlackHoleController.AddMass(IPullable)
public class EventHorizonController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver) return;
        PullableObject pullable = other.GetComponent<PullableObject>();
        if (pullable != null)
        {
            GameManager.Instance.blackHoleController.AddMass(pullable);
            pullable.Transform.DOScale(0, 0.3f).OnComplete(() => Destroy(other.gameObject));
        }
    }
}