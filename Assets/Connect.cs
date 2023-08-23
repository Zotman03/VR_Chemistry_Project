using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : MonoBehaviour
{
    private bool isBonded = false;

    void OnCollisionEnter(Collision collision)
    {
        Connect otherSphere = collision.gameObject.GetComponent<Connect>();
        if (otherSphere && !otherSphere.isBonded && !isBonded)
        {
            Bond(collision.gameObject);
            otherSphere.Bonded();
        }
    }

    void Bond(GameObject other)
    {
        // Create a fixed joint and attach the other object to it
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = other.GetComponent<Rigidbody>();
    }

    public void Bonded()
    {
        isBonded = true;
    }
}