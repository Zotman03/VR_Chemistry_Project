using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GetComponentInParent<Connect>().OnCollisionEnterChild(gameObject, collision);
    }
}

