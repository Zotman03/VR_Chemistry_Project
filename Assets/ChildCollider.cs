using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Call the parent's method to handle the collision
        // Make sure your parent GameObject has a script component that contains this method
        GetComponentInParent<Connect>().OnCollisionEnterChild(gameObject, collision);
    }
}

