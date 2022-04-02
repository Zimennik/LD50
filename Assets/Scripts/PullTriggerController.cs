using UnityEngine;


//this script is used to apply force to IPullable objects, that are in the trigger
//force power is based on the distance between the object and the black hole center
public class PullTriggerController : MonoBehaviour
{
    private BlackHoleController _blackHoleController;

    private void Start()
    {
        _blackHoleController = GameManager.Instance.blackHoleController;
    }

    private void OnTriggerStay(Collider other)
    {
        IPullable pullable = other.GetComponent<IPullable>();
        if (pullable != null)
        {
            pullable.Pull(_blackHoleController.transform);
        }
    }
}