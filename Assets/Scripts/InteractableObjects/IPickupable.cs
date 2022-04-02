using UnityEngine;

//objects with this interface can be picked up by the player
public interface IPickupable
{
    void PickUp();
    void Drop(Vector3 force);

    bool IsPickedUp { get; }
}